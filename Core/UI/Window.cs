using System;
using UnityEngine;

namespace JusticeFramework.Core.UI {
	public delegate void OnWindowShowing(Window window);
	public delegate void OnWindowShow(Window window);
	public delegate void OnWindowHiding(Window window);
	public delegate void OnWindowHide(Window window);
	public delegate void OnWindowClosing(Window window);
	public delegate void OnWindowClose(Window window);
	
	/// <inheritdoc />
	/// <summary>
	/// Base window class for ingame GUI panels
	/// </summary>
	[Serializable]
	public abstract class Window : MonoBehaviour {
		/// <summary>
		/// Event called before the window is shown
		/// </summary>
		public event OnWindowShowing OnWindowShowing;

		/// <summary>
		/// Event called after the window is shown
		/// </summary>
		public event OnWindowShow OnWindowShow;
		
		/// <summary>
		/// Event called before the window is shown
		/// </summary>
		public event OnWindowHiding OnWindowHiding;

		/// <summary>
		/// Event called after the window is shown
		/// </summary>
		public event OnWindowHide OnWindowHide;

		/// <summary>
		/// Event called after the window is hidden
		/// </summary>
		public event OnWindowClosing OnWindowClosing;
		
		/// <summary>
		/// Event called after the window is hidden
		/// </summary>
		public event OnWindowClose OnWindowClose;

		/// <summary>
		/// An Id number set by the window stack
		/// </summary>
		[SerializeField]
		private int id;
		
		/// <summary>
		/// Flag indicating if the window should be closed if another window is open on top of this one
		/// </summary>
		[SerializeField]
		private bool closesWhenCovered;

		/// <summary>
		/// Flag indicating if the window show hide the windows below it
		/// </summary>
		[SerializeField]
		private bool hidesWindowsBelow;

		/// <summary>
		/// Flag indicating if the window causes the game pause
		/// </summary>
		[SerializeField]
		private bool causesGamePause;

		/// <summary>
		/// Flag indicating if this window can be closed by any source other than the system
		/// </summary>
		[SerializeField]
		private bool canBeManuallyClosed;
		
		/// <summary>
		/// An Id number set by the window stack
		/// </summary>
		public int Id {
			get { return id; }
			set { id = value; }
		}

		/// <summary>
		/// Flag indicating if the window should be closed if another window is open on top of this one
		/// </summary>
		public bool ClosesWhenCovered {
			get { return closesWhenCovered; }
			set { closesWhenCovered = value; }
		}
		
		/// <summary>
		/// Flag indicating if the window show hide the windows below it
		/// </summary>
		public bool HidesWindowsBelow {
			get { return hidesWindowsBelow; }
			set { hidesWindowsBelow = value; }
		}
		
		/// <summary>
		/// Flag indicating if the window causes the game pause
		/// </summary>
		public bool CausesGamePause {
			get { return causesGamePause; }
			set { causesGamePause = value; }
		}
		
		/// <summary>
		/// Flag indicating if this window can be closed by any source other than the system
		/// </summary>
		public bool CanBeManuallyClosed {
			get { return canBeManuallyClosed; }
			set { canBeManuallyClosed = value; }
		}
		
		
		/// <summary>
		/// Flag indicating if the window is currently being shown
		/// </summary>
		public bool IsShowing { get; private set; }
		
		/// <summary>
		/// Internal method called when the window is shown
		/// </summary>
		protected virtual void OnShow() {
		}
		
		/// <summary>
		/// Internal method called when the window is hidden
		/// </summary>
		protected virtual void OnHide() {
		}

		/// <summary>
		/// Internal method called when the window is close
		/// </summary>
		protected virtual void OnClose() {
		}
		
		/// <summary>
		/// Shows the windows
		/// </summary>
		public void Show() {
			OnWindowShowing?.Invoke(this);
			
			OnShow();
			
			IsShowing = true;
			gameObject.SetActive(true);

			OnWindowShow?.Invoke(this);
		}

		/// <summary>
		/// Hides the window
		/// </summary>
		public void Hide() {
			OnWindowHiding?.Invoke(this);

			OnHide();
			
			gameObject.SetActive(false);

			OnWindowHide?.Invoke(this);
		}

		/// <summary>
		/// Closes the window by destroying the game object
		/// </summary>
		public void Close() {
			OnWindowClosing?.Invoke(this);

			OnClose();

			IsShowing = false;

			OnWindowClose?.Invoke(this);
			
			Destroy(gameObject);
		}
	}
}