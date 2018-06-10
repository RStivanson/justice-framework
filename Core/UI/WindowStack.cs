using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.UI {
	/// <summary>
	/// Window collection class that handles opening, closing, and overall managing the whole window stack.
	/// </summary>
	[Serializable]
	public class WindowStack : MonoBehaviour {
		public delegate void OnPauseRequested();
		public delegate void OnUnpauseRequested();

		/// <summary>
		/// Event called when a window is encountered that causes game pausing
		/// </summary>
		public event OnPauseRequested OnPause;
		
		/// <summary>
		/// Event called when all game pausing windows have been closed
		/// </summary>
		public event OnUnpauseRequested OnUnpause;
		
		/// <summary>
		/// The list of currently open/spawned windows
		/// </summary>
		[SerializeField]
		private List<Window> windows;

		/// <summary>
		/// A list of window prefabs that are to be assigned in the Unity Editor
		/// </summary>
		[SerializeField]
		private List<GameObject> windowPrefabs;

		/// <summary>
		/// The canvas to attach new windows to
		/// </summary>
		[SerializeField]
		private Transform uiCanvas;

		/// <summary>
		/// The processed window prefab, associated to their types
		/// </summary>
		[SerializeField]
		private Dictionary<Type, GameObject> windowPrefabsByType;

		/// <summary>
		/// Gets the amount of currently open windows
		/// </summary>
		public int Count {
			get { return windows.Count; }
		}
		
		/// <summary>
		/// Initializes the WindowStack when enabled
		/// </summary>
		public void Awake() {
            if (windows == null) {
                windows = new List<Window>();
            }

            ProcessWindowPrefabs();
		}

		/// <summary>
		/// Processes all window prefabs associated in the editor so that they can be spawned
		/// </summary>
		public void ProcessWindowPrefabs() {
			windowPrefabsByType = new Dictionary<Type, GameObject>();

			// For each prefab
			foreach (GameObject prefab in windowPrefabs) {
			    if (prefab == null) {
			    	continue;
			    }
			
				Window windowScript = prefab.GetComponent<Window>();

				// If the prefab doesn't have a window script, or the type has already
				// been associated then do nothing
				if (windowScript == null || windowPrefabsByType.ContainsKey(windowScript.GetType())) {
					continue;
				}
				
				// Add the window to the processed list
				windowPrefabsByType.Add(windowScript.GetType(), prefab);
			}
		}

		/// <summary>
		/// Gets the top most window of the given type
		/// </summary>
		/// <typeparam name="T">The type of window to get</typeparam>
		/// <returns>Return the top most window that matches the given type</returns>
		public T GetWindow<T>() where T : Window {
			T window = null;

			// For each window starting at the top, while we have no found the window
			for (int i = windows.Count - 1; i >= 0 && window == null; i--) {
				// If this is the window, then set it
				if (windows[i] is T) {
					window = (T)windows[i];
				}
			}
			
			return window;
		}
		
		/// <summary>
		/// Creates a new window of the given type
		/// </summary>
		/// <typeparam name="T">The type of window to create</typeparam>
		/// <returns>Returns the Window component from the spawned GUI object, null if it fails</returns>
		public T OpenWindow<T>() where T : Window {
			T window = null;
			GameObject prefab = null;

			// Get the prefab associated with this type then create it
			if (windowPrefabsByType.ContainsKey(typeof(T))) {
				prefab = windowPrefabsByType[typeof(T)];
			}
			
			if (prefab != null) {
				window = (T)CreateAndAppendWindow(prefab);
			}
			
			return window;
		}

		/// <summary>
		/// Creates and new window and adds it to the stack
		/// </summary>
		/// <param name="window">The prefab of the GUI GameObject to create</param>
		/// <returns>Returns the Window component on the newly spawned GUI GameObject</returns>
		private Window CreateAndAppendWindow(GameObject window) {
			// Spawn the window and get its Window component
			GameObject spawnedWindow = Instantiate(window);
			Window windowScript = spawnedWindow.GetComponent<Window>();

			// If the object has a window script do stuff, else destroy the object
			if (windowScript != null) {
				// Manage the windows that will be below this window
				ManageNewWindow(windowScript);

				// Assign the window an Id and subscribe to its OnClose
				windowScript.Id = windows.Count;
				windowScript.OnWindowClose += OnWindowClosed;

				// Attach the spawned object to the canvas and the window stack then show it
				spawnedWindow.transform.SetParent(uiCanvas, false);
				windows.Add(spawnedWindow.GetComponent<Window>());
				windowScript.Show();
			} else {
				Destroy(spawnedWindow);
			}

			return windowScript;
		}

		/// <summary>
		/// Closes the top window on the stack
		/// </summary>
		public void CloseTop(bool force = false) {
			// If there are no open windows, do nothing
			if (windows.Count == 0) {
				return;
			}

			// Get the top window
			Window window = Peek();

			// If this window can be manually closed, or is being forced to be closed
			if (window.CanBeManuallyClosed || force) {
				window.Close();
			}
		}
		
		/// <summary>
		/// Closes all currently open windows
		/// </summary>
		public void CloseAllWindows() {
			// While there are still windows on the stack, close the top one
			while (windows.Count != 0) {
				CloseTop(true);
			}
		}
		
		/// <summary>
		/// Gets the window on the top of window stack
		/// </summary>
		/// <returns>The window on top of the stack</returns>
		public Window Peek() {
			return (windows.Count != 0) ? windows[windows.Count - 1] : null;
		}

		/// <summary>
		/// Manages updating and hiding other windows
		/// </summary>
		/// <param name="newWindow">The window that has been opened</param>
		private void ManageNewWindow(Window newWindow) {
			// If this new window says it should hide the windows below it
			if (newWindow.HidesWindowsBelow) {
				// For each window starting at the top
				for (int i = windows.Count - 1; i >= 0; i--) {
					// If this window wants to be closed when covered then close it
					if (windows[i].ClosesWhenCovered) {
						windows[i].Close();
					} else {
						// If the window doesn't want closed then hide it
						windows[i].Hide();
					}

					// If this window hides below, then we know the below windows are
					// already hidden so we can stop now
					if (windows[i].HidesWindowsBelow) {
						break;
					}
				}
			}

			if (newWindow.CausesGamePause) {
				OnPause?.Invoke();
			}
		}

		/// <summary>
		/// Manages updating and showing other windows that may have been affected by this window
		/// </summary>
		/// <param name="closedWindow">The window that has been closed</param>
		private void ManageClosedWindow(Window closedWindow) {
			bool hasAHideAbove = HasAHideWindowAbove(closedWindow.Id);
			bool hasAPauseElsewhere = HasAPauseElsewhere(closedWindow.Id);
			int i;

			// Check if there are any windows above this one that is hiding what is below it
			for (i = closedWindow.Id + 1; i < windows.Count; i++) {
				hasAHideAbove = windows[i].HidesWindowsBelow;

				if (hasAHideAbove) {
					break;
				}
			}

			// If there is nothing hiding what is below it, then show all windows until
			// we find a window that is
			if (!hasAHideAbove) {
				// For each window starting at the window below the closed one
				for (i = closedWindow.Id - 1; i >= 0; i--) {
					windows[i].Show();
					
					if (windows[i].HidesWindowsBelow) {
						break;
					}
				}
			}

			// If there is no other pausing windows and this window caused pausing, then send off
			// an unpause event
			if (!hasAPauseElsewhere && closedWindow.CausesGamePause) {
				OnUnpause?.Invoke();
			}
		}

		/// <summary>
		/// Iterates through all open windows and reassigns them a new Id based on there index
		/// </summary>
		private void RefreshWindowIds() {
			for (int i = 0; i < windows.Count; i++) {
				windows[i].Id = i;
			}
		}

		/// <summary>
		/// Searches the list above the given window to find out if any window is hiding the windows below it
		/// </summary>
		/// <param name="windowIndex">The index of the window to search away from</param>
		/// <returns>Returns true if a second above the current is hiding what is below it, false otherwise</returns>
		private bool HasAHideWindowAbove(int windowIndex) {
			bool hasAHideAbove = false;

			// Check if there are any windows above this one that is hiding what is below it
			for (int i = windowIndex + 1; i < windows.Count && !hasAHideAbove; i++) {
				hasAHideAbove = windows[i].HidesWindowsBelow;
			}

			return hasAHideAbove;
		}
		
		/// <summary>
		/// Searches the list of open windows to see if there is any other window open that is
		/// causing the game to pause
		/// </summary>
		/// <param name="windowIndex">The index of the window to search away from</param>
		/// <returns>Returns true if a second window that causes pausing exists, false otherwise</returns>
		private bool HasAPauseElsewhere(int windowIndex) {
			bool foundPause = false;
			
			// Search the list to find a window that is causing game pausing
			for (int i = 0; i < windows.Count && !foundPause; i++) {
				// If the index is the same as the starting window, then skip
				if (i == windowIndex) {
					continue;
				}
				
				// Get this windows pause value
				foundPause = windows[i].CausesGamePause;
			}

			return foundPause;
		}
		
#region Event Callbacks

		/// <summary>
		/// Event handler called when any window is closed, handles removing it from the list and updating the rest of the windows
		/// </summary>
		/// <param name="window">The window that has been closed</param>
		private void OnWindowClosed(Window window) {
			// Update the other open windows then remove this window
			ManageClosedWindow(window);
			windows.RemoveAt(window.Id);

			// Make sure all windows still accurately represent their position in the stack
			RefreshWindowIds();
		}

#endregion
	}
}