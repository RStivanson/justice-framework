using System;
using JusticeFramework.Core.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using JusticeFramework.Core.Managers.Resources;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Console;
using JusticeFramework.Core.UI.Views;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JusticeFramework.Core.Managers {
    public delegate void OnPauseStateChanged(bool isPaused);
	
	[Serializable]
	public class GameManager : MonoBehaviour {
		public event OnPauseStateChanged OnGamePause;
		
#region Variables

		[SerializeField]
		private static GameManager gameManager;
		
		[SerializeField]
		protected IActor player;

		[SerializeField]
		protected static AssetManager assetManager;
		
		[SerializeField]
		protected static DialogueManager dialogueManager;

        [SerializeField]
        protected static QuestManager questManager;

        [SerializeField]
        protected static RecipeManager recipeManager;

        [SerializeField]
        protected static SettingsManager settingsManager;

        [SerializeField]
		protected CommandLibrary commandLibrary;
		
		[SerializeField]
		protected DateTime gameTime;

		[SerializeField]
		protected bool isPaused;
		
#endregion

#region Properties

		public static GameManager Instance {
			get { return gameManager; }
		}
		
		public static IActor Player {
			get {
                if (!IsPlaying) {
                    return null;
                }

				if (Instance.player == null) {
					Instance.player = Spawn(SystemConstants.AssetDataPlayerId, Vector3.zero, Quaternion.identity) as IActor;
				}

				return Instance.player;
			}
		}
		
		public static bool IsPaused {
			get { return Instance?.isPaused ?? false; }

			private set {
				if (Instance.isPaused == value) {
					return;
				}

				Instance.isPaused = value;

				if (Instance.isPaused) {
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				} else {
					Cursor.lockState = CursorLockMode.Locked;
				}

				Instance.OnGamePause?.Invoke(Instance.isPaused);
			}
		}
		
		public static AssetManager AssetManager {
			get { return assetManager; }
		}

		public static DialogueManager DialogueManager {
			get { return dialogueManager; }
        }

        public static QuestManager QuestManager {
            get { return questManager; }
        }

        public static RecipeManager RecipeManager {
            get { return recipeManager; }
        }

        public static SettingsManager SettingsManager {
            get { return settingsManager; }
        }

        public DateTime GameTime {
			get { return gameTime; }
		}

		public static CommandLibrary CommandLibrary {
			get { return Instance.commandLibrary; }
			private set { Instance.commandLibrary = value; }
		}
		
		public static bool IsPlaying {
			get; protected set;
		}

#endregion

		private void Awake() {
            if (gameManager != null) {
                Debug.LogError($"GameManager - There can only be one game manager active at once, destroying self. (object name: {name})");
                Destroy(gameObject);
                return;
            }

			gameManager = this;
			
			assetManager = new AssetManager();
            dialogueManager = new DialogueManager();
            questManager = new QuestManager();
            recipeManager = new RecipeManager();
            settingsManager = new SettingsManager();

            assetManager.LoadResources();
            dialogueManager.LoadResources();
			questManager.LoadResources();
            recipeManager.LoadResources();
            settingsManager.LoadResources();

            CommandLibrary = new CommandLibrary();

            OnInitialized();
		}

        /// <summary>
        /// Internal method called when the object is initialized
        /// </summary>
        protected virtual void OnInitialized() {
		}

		public virtual void BeginGame() {
		}
		
		public virtual void EndGame() {
		}
		
		public static void ExitGame() {
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
		
#region Cell Functions

		[ConsoleCommand("sts", "Sends the player to the specified scene at the given coordinates")]
		public static void SendToScene(string sceneName, Vector3 position) {
			UnloadAllLevels();
			LoadLevel(sceneName, () => {
				Player.Transform.position = position;
			});
		}
		
		[ConsoleCommand("sts", "Sends the player to the specified scene at the given coordinates")]
		public static void SendToScene(string sceneName, Vector3 position, Vector3 rotation) {
			UnloadAllLevels();
			LoadLevel(sceneName, () => {
				Player.Transform.position = position;
				Player.Transform.eulerAngles = rotation;
			});
		}
		
		[ConsoleCommand("tele", "Teleports the world object to the specified coordinates")]
		private static void TeleportWorldObject(IWorldObject reference, Vector3 position) {
			reference.Transform.position = position;
		}

#endregion
		
#region Pause Functions

		public static void Pause() {
			IsPaused = true;
			Time.timeScale = 0;
		}

		public static void Unpause() {
			IsPaused = false;
			Time.timeScale = 1;
		}

#endregion
		
#region Spawning Functions

		[ConsoleCommand("spawn", "Spawns a new copy of the specified asset")]
		public static IWorldObject Spawn(string id) {
			return Spawn(id, Vector3.zero, Quaternion.identity);
		}
		
		[ConsoleCommand("spawn", "Spawns a new copy of the specified asset with the given position")]
		public static IWorldObject Spawn(string id, Vector3 spawnPos) {
			return Spawn(id, spawnPos, Quaternion.identity);
		}
		
		[ConsoleCommand("spawn", "Spawns a new copy of the specified asset with the given position and rotation")]
		public static IWorldObject Spawn(string id, Vector3 position, Quaternion rotation) {
			return Spawn(assetManager.GetById(id), position, rotation);
		}
		
		public static IWorldObject Spawn(WorldObjectModel worldObjectModel, Vector3 position, Quaternion rotation) {
			if (worldObjectModel != null) {
				GameObject obj = Instantiate(worldObjectModel.prefab, position, rotation);

				if (obj != null) {
                    IWorldObject reference = obj.GetComponent<IWorldObject>();

					reference.SetData(worldObjectModel, true);

					return reference;
				}
			}

			return null;
		}

		[ConsoleCommand("spawnatplayer", "Spawns a new copy of the specified asset next to the player")]
		private static void SpawnAtPlayer(string id) {
			Transform playerTransform = Player.Transform;
			Vector3 spawnPos = playerTransform.position + (playerTransform.forward * settingsManager.GetFloat(SystemConstants.SettingItemDropSpawnY));
			spawnPos.y += settingsManager.GetFloat(SystemConstants.SettingItemDropSpawnForward);

			Spawn(id, spawnPos);
		}

#endregion
		
#region Scene Management

        /// <summary>
        /// Loads a specific scene
        /// </summary>
        /// <param name="sceneName">The name of the scene to load</param>
        /// <param name="onLoadComplete">Callback to be called when the scene has finished loading</param>
        /// <param name="showLoadScreen">Flag indicating if the loading screen should be shown</param>
		protected static void LoadLevel(string sceneName, UnityAction onLoadComplete = null, bool showLoadScreen = true) {
			// Close all open windows and all open scenes
			UiManager.UI.CloseAllWindows();
			UnloadAllLevels();
			
			LoadView view = UiManager.UI.OpenWindow<LoadView>();
			view.Monitor(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive), () => {
                // Set the new scene to be the active scene
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

                // Update the global illumination so lighting comes from the active scene
				DynamicGI.UpdateEnvironment();

                // The game is now playing
                IsPlaying = true;

                Instance.OnLevelLoaded();
				
                // Let whoever is listening know that a level has been loaded
				onLoadComplete?.Invoke();
			});

            // If we are not showing the load screen, hide it
			if (!showLoadScreen) {
				view.Hide();
			}
		}

        /// <summary>
        /// Called when a level has finished loading
        /// </summary>
        protected virtual void OnLevelLoaded() {
        }

        /// <summary>
        /// Unloads all scenes except for the main scene
        /// </summary>
        /// <param name="mainSceneName">Main scene with the game manager instance</param>
		protected static void UnloadAllLevels(string mainSceneName = null) {
            // If the provided scene is empty or null
            if (string.IsNullOrEmpty(mainSceneName)) {
                // Get the default setting from the setting manager
                mainSceneName = settingsManager.GetString(SystemConstants.SettingSceneMainScene);
            }

            // For each loaded scene
			for (int i = SceneManager.sceneCount - 1; i > 0; i--) {
				Scene scene = SceneManager.GetSceneAt(i);

                // If this scene is not the main scene, unload it
				if (!scene.name.Equals(mainSceneName)) {
					SceneManager.UnloadSceneAsync(scene);
				}
			}
		}

#endregion
	}
}
