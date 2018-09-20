using JusticeFramework.Core.UI;
using JusticeFramework.Core.UI.Views;
using System;
using UnityEngine;

namespace JusticeFramework.Core.Managers {
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
	}
}
