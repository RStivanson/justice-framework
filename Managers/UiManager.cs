using JusticeFramework.Core.UI;
using JusticeFramework.UI.Views;
using System;
using UnityEngine;

namespace JusticeFramework.Managers {
    [Serializable]
    [DefaultExecutionOrder(-99)]
	public class UiManager : MonoBehaviour {
		[SerializeField]
		private static UiManager uiManager;
		
		[SerializeField]
		protected WindowStack uiWindowStack;
		
		public static UiManager Instance {
			get { return uiManager; }
		}
		
		public static WindowStack UI {
			get { return Instance.uiWindowStack; }
		}
		
		private void Awake() {
            if (uiManager != null) {
                Debug.LogError($"UIManager - There can only be one UI manager active at once, destroying self. (object name: {name})");
                Destroy(gameObject);
                return;
            }

			uiManager = this;
            uiWindowStack.ProcessWindowPrefabs();

			UI.onPause += GameManager.Pause;
			UI.onUnpause += GameManager.Unpause;
		}
		
		private void Start() {
			OnInitialized();
		}

		protected virtual void OnInitialized() {
		}

        public static void Notify(string notification) {
            HudView hud = UiManager.UI.GetWindow<HudView>();

            if (hud != null) {
                hud.ShowNotification(notification);
            }
        }
    }
}
