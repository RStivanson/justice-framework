using System;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.UI;
using UnityEngine;

namespace JusticeFramework.UI.Views {
	/// <summary>
	/// Main menu window class
	/// </summary>
	[Serializable]
	public class MainMenuView : Window {
		[SerializeField]
		private Transform buttonContainer;

		[SerializeField]
		private GameObject buttonPrefab;

        #region Button Callbacks

		public void NewGame() {
            GameManager.Instance.BeginGame();
        }

        public void OpenSettings() {
            UiManager.UI.OpenWindow<SettingsView>();
        }

        public void ExitGame() {
			GameManager.ExitGame();
		}

        #endregion
	}
}