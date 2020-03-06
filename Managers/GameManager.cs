using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using JusticeFramework.Interfaces;
using JusticeFramework.Console;
using JusticeFramework.UI.Views;
using JusticeFramework.Data;
using JusticeFramework.Core.Extensions;
using JusticeFramework.Components;
using JusticeFramework.Logic;
using JusticeFramework.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JusticeFramework.Managers
{
    public delegate void OnPauseStateChanged(bool isPaused);
	
	[Serializable]
    [DefaultExecutionOrder(-100)]
	public class GameManager : MonoBehaviour {
		public event OnPauseStateChanged onGamePause;

        [SerializeField]
        private AudioClip ambientMusic;

        [SerializeField]
        private AudioSource ambientAudioSource;

        private Actor player;
		private bool isPaused;
		
		public static GameManager Instance {
            get; private set;
		}

        public static AudioSource AmbientSource {
            get { return Instance.ambientAudioSource; }
        }

        public static DataManager DataManager {
            get; private set;

        }

		public static CommandLibrary CommandLibrary {
            get; private set;
		}

        public static bool IsPaused {
            get { return Instance.isPaused; }
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

                Instance.onGamePause?.Invoke(Instance.isPaused);
            }
        }

        public static bool IsPlaying {
			get; private set;
		}

        private void Awake() {
            if (Instance != null) {
                Debug.LogError($"GameManager - There can only be one game manager active at once, destroying self. (object name: {name})");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            CommandLibrary = new CommandLibrary();
            CommandLibrary.LoadCommands();
            DataManager = new DataManager();
            DataManager.LoadData();

            // TODO
            //SystemSettings.Load(Application.persistentDataPath, "settings", true);
            //SystemSettings.ApplySettings();
        }

        public void Start() {
            SceneManager.LoadScene(EngineSettings.SettingSceneMainMenu, LoadSceneMode.Single);
            UiManager.UI.OpenWindow<MainMenuView>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (IsPlaying) {
                    if (UiManager.UI.Peek() is HudView) {
                        UiManager.UI.OpenWindow<SystemMenuView>();
                    } else {
                        UiManager.UI.CloseTop();
                    }
                } else {
                    if (!(UiManager.UI.Peek() is MainMenuView)) {
                        UiManager.UI.CloseTop();
                    }
                }
            }

            if (!IsPlaying) {
                return;
            }

            if (Input.GetKeyDown(KeyCode.K)) {
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if (UiManager.UI.Peek() is HudView) {
                    ContainerView view = UiManager.UI.OpenWindow<ContainerView>();
                    view.View(GetPlayer(), null, targetMask: EContainerViewMask.Items);
                }
            }

            if (Input.GetKeyDown(KeyCode.F)) {
                if (UiManager.UI.Peek() is HudView) {
                    CraftingView crafting = UiManager.UI.OpenWindow<CraftingView>();
                    crafting.View(GetPlayer());
                }
            }

            if (Input.GetKeyDown(KeyCode.BackQuote)) {
                if (UiManager.UI.Peek().NotType<ConsoleView>()) {
                    ConsoleView view = UiManager.UI.OpenWindow<ConsoleView>();
                    view.SetCommandLibrary(CommandLibrary);
                }
            }

            if (false && Input.GetKeyDown(KeyCode.R)) {
                if (UiManager.UI.Peek() is HudView) {
                    QuestView view = UiManager.UI.OpenWindow<QuestView>();
                }
            }
        }

        #region Game State Methods

        public virtual void BeginGame() {
            LoadLevel(EngineSettings.SettingSceneNewGameStart, OnPostBeginGame);
        }
		
        public virtual void OnPostBeginGame() {
            //SpawnPlayer(new Vector3(-719.5f, 50, -53.25f), Quaternion.identity);
            SpawnPlayer(new Vector3(-5, 0.1f, -1), Quaternion.identity);
            CommandLibrary.SetInteractionController(Instance.player.GetComponent<IInteractionController>());

            if (ambientMusic != null) {
                ambientAudioSource.clip = ambientMusic;
                ambientAudioSource.Play();
            }

            Unpause();
        }

		public virtual void EndGame() {
            IsPlaying = false;

            Destroy(player.Transform.gameObject);
        }
		
		public static void ExitGame() {
        #if UNITY_EDITOR
			EditorApplication.isPlaying = false;
        #else
			Application.Quit();
        #endif
        }

        #endregion

        #region Player Methods

        /// <summary>
        /// Spawns the player prefab and assigns it to the instance
        /// </summary>
        /// <returns>Returns the player's Actor script</returns>
        private Actor SpawnPlayer(Vector3 position, Quaternion rotation) {
            Instance.player = Spawn(EngineSettings.SettingIDPlayer, position, rotation) as Actor;
            return Instance.player;
        }

        /// <summary>
        /// Gets the player's Actor script.
        /// </summary>
        /// <returns>Returns the player's Actor script, null if the player isn't spawned.</returns>
        public static Actor GetPlayer() {
            if (!IsPlaying) {
                return null;
            }

            return Instance.player;
        }

        /// <summary>
        /// Determines if the given object is the player
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Returns a flag indicating if the object is the player</returns>
        public static bool IsPlayer(WorldObject obj) {
            return ReferenceEquals(Instance.player, obj);
        }

        #endregion

        #region Event Callbacks

        protected virtual void OnQuestUpdated(string id, int marker) {
            string questDisplayName = "Unkown";//DataManager.GetQuestDisplayName(id);
            UiManager.Notify($"{questDisplayName} updated");
        }

        #endregion

        #region Cell Methods

        [ConsoleCommand("sts", "Sends the player to the specified scene at the given coordinates")]
		public static void SendPlayerToScene(string sceneName, Vector3 position) {
			UnloadAllLevels();
			LoadLevel(sceneName, () => {
                GetPlayer().Transform.position = position;
			});
		}
		
		[ConsoleCommand("sts", "Sends the player to the specified scene at the given coordinates")]
		public static void SendPlayerToScene(string sceneName, Vector3 position, Vector3 rotation) {
			UnloadAllLevels();
			LoadLevel(sceneName, () => {
				GetPlayer().Transform.position = position;
                GetPlayer().Transform.eulerAngles = rotation;
			});
		}
		
		[ConsoleCommand("tele", "Teleports the world object to the specified coordinates")]
		private static void TeleportWorldObject(IWorldObject reference, Vector3 position) {
            TeleportTransform(reference.Transform, position);
		}
        
		[ConsoleCommand("tele", "Teleports the transform to the specified coordinates")]
		private static void TeleportTransform(Transform transform, Vector3 position) {
			transform.position = position;
		}

        #endregion
		
        #region Pause Methods

		public static void Pause() {
			IsPaused = true;
			Time.timeScale = 0;
		}

		public static void Unpause() {
			IsPaused = false;
			Time.timeScale = 1;
		}

        #endregion

        #region Spawning Methods

		[ConsoleCommand("spawn", "Spawns a new copy of the specified asset with the given position and rotation")]
		public static IWorldObject Spawn(string id, Vector3 position, Quaternion rotation) {
			return Spawn(DataManager.GetAssetById(id), position, rotation);
        }

        public static IWorldObject Spawn(ScriptableDataObject dataModel, Vector3 position, Quaternion rotation) {
            IWorldObject worldObject = Spawn((dataModel as ISceneDataObject).ScenePrefab, position, rotation);
            worldObject.SetData(dataModel);
            return worldObject;
        }

        public static IWorldObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) {
            IWorldObject result = null;

            if (prefab != null) {
                GameObject obj = Instantiate(prefab, position, rotation);

                if (obj != null) {
                    result = obj.GetComponent<IWorldObject>();
                }
            }

            return result;
        }

        [ConsoleCommand("spawnatme", "Spawns a new copy of the specified asset next to the player")]
        public static IWorldObject SpawnAtPlayer(string id) {
            return SpawnAtActor(DataManager.GetAssetById<ItemData>(id), GetPlayer());
        }

        public static IWorldObject SpawnAtActor(ItemData id, Actor actor) {
            Transform actorTransform = actor.Transform;
            Vector3 spawnPos = actorTransform.position + (actorTransform.forward * EngineSettings.SettingSpawnAtActorForwardMultipler);
            spawnPos.y += EngineSettings.SettingSpawnAtActorVerticalOffset;

            return Spawn(id, spawnPos, Quaternion.identity);
        }

        /*public static IEquippable SpawnEquipment(string id) {
            return SpawnEquipment(AssetManager.GetById<EquippableModel>(id));
        }

        public static IEquippable SpawnEquipment(EquippableModel equippable) {
            IEquippable result = null;

            if (equippable != null) {
                result = (IEquippable)Spawn(equippable.equipmentPrefab, Vector3.zero, Quaternion.identity);
            }

            return result;
        }*/

        #endregion

        #region Scene Management Methods

        /// <summary>
        /// Loads a specific scene
        /// </summary>
        /// <param name="sceneName">The name of the scene to load</param>
        /// <param name="onLoadComplete">Callback to be called when the scene has finished loading</param>
        /// <param name="showLoadScreen">Flag indicating if the loading screen should be shown</param>
        private static void LoadLevel(string sceneName, UnityAction onLoadComplete = null, bool showLoadScreen = true) {
			// Close all open windows and all open scenes
			UiManager.UI.CloseAllWindows();
			UnloadAllLevels();
			
			LoadView view = UiManager.UI.OpenWindow<LoadView>();
			view.Monitor(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single), () => {
                // Set the new scene to be the active scene
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

                // Update the global illumination so lighting comes from the active scene
                DynamicGI.UpdateEnvironment();

                // The game is now playing
                IsPlaying = true;

                // Let whoever is listening know that a level has been loaded
				onLoadComplete?.Invoke();

                // Close the loading screen, and open the HUD
                UiManager.UI.CloseTop(true);
                UiManager.UI.OpenWindow<HudView>().SetPlayer(GetPlayer());
            });

            // If we are not showing the load screen, hide it
			if (!showLoadScreen) {
				view.Hide();
			}
		}

        /// <summary>
        /// Unloads all scenes
        /// </summary>
		private static void UnloadAllLevels() {
            // For each loaded scene
			for (int i = SceneManager.sceneCount - 1; i > 0; i--) {
				Scene scene = SceneManager.GetSceneAt(i);
				SceneManager.UnloadSceneAsync(scene);
			}
		}

        #endregion
	}
}
