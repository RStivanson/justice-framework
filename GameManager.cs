using System;
using System.IO;
using System.Xml.Serialization;
using JetBrains.Annotations;
using JusticeFramework.AI;
using JusticeFramework.Data.Models;
using JusticeFramework.Components;
using JusticeFramework.Console;
using JusticeFramework.Data.Dialogue;
using JusticeFramework.UI.Views;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
	using UnityEditor;
#endif

namespace JusticeFramework {
	public delegate void OnPauseStateChanged(bool isPaused);
	
	[Serializable]
	public class GameManager : MonoBehaviour {
		public event OnPauseStateChanged OnGamePause;
		
#region Variables

		[SerializeField]
		private static GameManager gameManager;
		
		[SerializeField]
		protected Actor player;

		[SerializeField]
		protected static AssetManager assetManager;
		
		[SerializeField]
		protected static DialogueManager dialogueManager;
		
		[SerializeField]
		protected static QuestManager questManager;
		
		[SerializeField]
		protected static ReferenceManager referenceManager;
		
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
		
		public static Actor Player {
			get {
				if (Instance.player == null) {
					Instance.player = Spawn("ActorPlayer", Vector3.zero, Quaternion.identity).GetComponent<Actor>();
				}

				return Instance.player;
			}
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
			gameManager = this;
			
			assetManager = new AssetManager();
			assetManager.LoadAssets();
			
			dialogueManager = new DialogueManager();
			dialogueManager.Initialize();
			
			questManager = new QuestManager();
			questManager.Initialize();
			
			CommandLibrary = new CommandLibrary();
			
			DialogueTree tree = new DialogueTree();

			tree.AddNode(EDialogueType.Player, "How are you?", false, false, true); // 0
			int i = tree.AddNode(EDialogueType.Npc, "I'm good.", false, false); // 1
			tree.AddNode(EDialogueType.Npc, "I'm alright.", false, false); // 2
			tree.AddNode(EDialogueType.Npc, "Just peachy, go away!", true, true); // 3
			tree.AddNode(EDialogueType.Player, "Any rumors?", false, false); // 4
			tree.AddNode(EDialogueType.Npc, "None lately.", false, true); // 5
			tree.AddNode(EDialogueType.Npc, "There has been talk of bandits recently.", false, true); // 6
			tree.AddNode(EDialogueType.Player, "Just alright?", false, false); // 7
			tree.AddNode(EDialogueType.Npc, "Yes, now go away!", true, true); // 8

			tree.AddEdge(true, 0, 1);
			tree.AddEdge(true, 0, 2);
			tree.AddEdge(true, 0, 3);
			tree.AddEdge(true, 2, 7);
			tree.AddEdge(true, 1, 4);
			tree.AddEdge(true, 4, 5);
			tree.AddEdge(true, 4, 6);
			tree.AddEdge(true, 7, 8);

			DialogueNode good = tree.GetNode(i);
			
			string json = JsonUtility.ToJson(tree);
			Debug.Log(json);
			File.WriteAllText("test.json", json);
		}
		
		private void Start() {
			OnInitialize();
		}

		protected virtual void OnInitialize() {
			
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
		
		public void Register(Reference reference) {
			referenceManager?.RegisterReference(reference);
		}

		public void Unregister(Reference reference) {
			referenceManager?.UnregisterReference(reference);
		}

#region Cell Functions

		[ConsoleCommand("sts", "Sends the player to the specified scene at the given coordinates")]
		public static void SendToScene(string sceneName, Vector3 position) {
			UnloadAllLevels();
			LoadLevel(sceneName, () => {
				Player.transform.position = position;
			});
		}
		
		[ConsoleCommand("sts", "Sends the player to the specified scene at the given coordinates")]
		public static void SendToScene(string sceneName, Vector3 position, Vector3 rotation) {
			UnloadAllLevels();
			LoadLevel(sceneName, () => {
				Player.transform.position = position;
				Player.transform.eulerAngles = rotation;
			});
		}
		
		[ConsoleCommand("tele", "Teleports the reference to the specified coordinates")]
		private static void TeleportReference(Reference reference, Vector3 position) {
			reference.transform.position = position;
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
		public static Reference Spawn(string id) {
			return Spawn(id, Vector3.zero, Quaternion.identity);
		}
		
		[ConsoleCommand("spawn", "Spawns a new copy of the specified asset with the given position")]
		public static Reference Spawn(string id, Vector3 spawnPos) {
			return Spawn(id, spawnPos, Quaternion.identity);
		}
		
		[ConsoleCommand("spawn", "Spawns a new copy of the specified asset with the given position and rotation")]
		public static Reference Spawn(string id, Vector3 position, Quaternion rotation) {
			return Spawn(assetManager.GetEntityById(id), position, rotation);
		}
		
		public static Reference Spawn(WorldObjectModel worldObjectModel, Vector3 position, Quaternion rotation) {
			if (worldObjectModel != null) {
				GameObject obj = Instantiate(worldObjectModel.prefab, position, rotation);

				if (obj != null) {
					Reference reference = obj.GetComponent<Reference>();

					reference.SetData(worldObjectModel, true);

					return reference;
				}
			}

			return null;
		}

		[ConsoleCommand("spawnatplayer", "Spawns a new copy of the specified asset next to the player")]
		private static void SpawnAtPlayer(string id) {
			Transform playerTransform = Player.transform;
			Vector3 spawnPos = playerTransform.position + (playerTransform.forward * 2.0f);
			spawnPos.y += 1.75f;

			Spawn(id, spawnPos);
		}

#endregion
		
#region Scene Management

		protected static void LoadLevel(string sceneName, UnityAction onLoadComplete = null, bool showLoadScreen = true) {
			// Close all open windows and all open scenes
			UiManager.UI.CloseAllWindows();
			UnloadAllLevels();
			
			LoadView view = UiManager.UI.OpenWindow<LoadView>();
			view.Monitor(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive), () => {
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
				DynamicGI.UpdateEnvironment();
				
				// Close the loading screen
				UiManager.UI.CloseTop(true);
				
				// Open the HUD window for the player
				UiManager.UI.OpenWindow<HudView>().SetPlayer(Player);
				
				IsPlaying = true;
				
				onLoadComplete?.Invoke();
			});

			if (!showLoadScreen) {
				view.Hide();
			}
		}

		protected static void UnloadAllLevels(string mainSceneName = "MainScene") {
			for (int i = SceneManager.sceneCount - 1; i > 0; i--) {
				Scene scene = SceneManager.GetSceneAt(i);

				if (!scene.name.Equals(mainSceneName)) {
					SceneManager.UnloadSceneAsync(scene);
				}
			}
		}

#endregion
	}
}
