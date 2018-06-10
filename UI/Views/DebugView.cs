using JusticeFramework.Core.Controllers;
using JusticeFramework.Core.Managers;
using System;
using UnityEngine;

namespace JusticeFramework.UI.Views {
    [Serializable]
	public class DebugView : MonoBehaviour {
		private enum DebugTab {
			System,
			Game
		}

		[SerializeField]
		private bool isShowing;
		
		[SerializeField]
		private DebugTab currentTab;
		
		private float deltaFrameTime;

		public float FramesPerSecond {
			get { return 1.0f / deltaFrameTime;  }
		}

		public float MillisecondsBetweenFrames {
			get { return deltaFrameTime * 1000.0f; }
		}
		
		private void Awake() {
			isShowing = false;
			currentTab = DebugTab.System;
			deltaFrameTime = 0;
		}
		
		private void Update() {
			deltaFrameTime += (Time.unscaledDeltaTime - deltaFrameTime) * 0.1f;
			
			if (Input.GetKeyDown(KeyCode.F11)) {
				isShowing = !isShowing;
			}
		}

		private void OnGUI() {
			if (!isShowing) {
				return;
			}
			
			GUILayout.BeginArea(new Rect(Screen.width - 10 - 250, 10, 250, 400));
			GUILayout.BeginVertical();
			
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("System")) {
				currentTab = DebugTab.System;
			}
			
			if (GUILayout.Button("Game")) {
				currentTab = DebugTab.Game;
			}
			
			GUILayout.EndHorizontal();
			
			switch (currentTab) {
				case DebugTab.System:
					DrawSystemTab();
					break;
				case DebugTab.Game:
					DrawGameTab();
					break;
				default:
					Debug.LogError($"Unknown debug tab: {currentTab}");
					break;
			}
     
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		
		protected virtual void DrawSystemTab() {
			GUILayout.BeginVertical("Box");
			
			GUILayout.Label($"FPS: {FramesPerSecond}");
			GUILayout.Label($"TFPS: {Application.targetFrameRate}");
			GUILayout.Label($"Msec: {MillisecondsBetweenFrames}");
			GUILayout.Label($"VSync: {QualitySettings.vSyncCount > 0} ({QualitySettings.vSyncCount})");
			
			GUILayout.EndVertical();
		}
		
		protected virtual void DrawGameTab() {
			GUILayout.BeginVertical("Box");
			
			if (GameManager.IsPlaying) {
				InteractionController controller = GameManager.Player.Transform.GetComponent<InteractionController>();
				
				GUILayout.Label($"Player Coord: {GameManager.Player.Transform.position}");
				GUILayout.Label($"LookAt Id: {controller.CurrentTarget?.Id}");
				GUILayout.Label($"LookAt Type: {controller.CurrentTarget?.GetType().Name}");
			} else {
				GUILayout.Label("Player Coord: --");
				GUILayout.Label("LookAt Id: --");
				GUILayout.Label("LookAt Type: --");
			}
			

			GUILayout.EndVertical();
		}
	}
}