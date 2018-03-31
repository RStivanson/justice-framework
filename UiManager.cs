using System;
using JusticeFramework.Components;
using JusticeFramework.Console;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Data.Models;
using JusticeFramework.UI;
using JusticeFramework.UI.Views;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework {
	[Serializable]
	public class UiManager : MonoBehaviour {
#region Variables

		[SerializeField]
		private static UiManager uiManager;
		
		[SerializeField]
		protected WindowStack uiWindowStack;
		
#endregion

#region Properties

		public static UiManager Instance {
			get { return uiManager; }
		}
		
		public static WindowStack UI {
			get { return Instance.uiWindowStack; }
		}
		
#endregion

		private void Awake() {
			uiManager = this;
			
			UI.OnPause += GameManager.Pause;
			UI.OnUnpause += GameManager.Unpause;
		}
		
		private void Update() {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				if (GameManager.IsPlaying && UI.Peek() is HudView) {
					UI.OpenWindow<SystemMenuView>();
				} else {
					UI.CloseTop();
				}
			}

			if (Input.GetKeyDown(KeyCode.Tab)) {
				if (GameManager.IsPlaying && UI.Peek() is HudView) {
					ContainerView view = UI.OpenWindow<ContainerView>();
					view.View(GameManager.Player, null);
				}
			}
		
			if (Input.GetKeyDown(KeyCode.BackQuote)) {
				if (UI.Peek().NotType<ConsoleView>()) {
					ConsoleView view = UI.OpenWindow<ConsoleView>();
					view.SetCommandLibrary(GameManager.CommandLibrary);
				}
			}
		}

		private void Start() {
			OnInitialize();
		}

		protected virtual void OnInitialize() {
		}
	}
}
