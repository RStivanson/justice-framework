using System;
using JusticeFramework.UI;
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

		[SerializeField]
		[HideInInspector]
		private GameManager gameManager;

		public void SetGameManager(GameManager manager) {
			gameManager = manager;
		}
		
#region Button Callbacks

		public void NewGame() {
			gameManager?.BeginGame();
		}

		public void ExitGame() {
			GameManager.ExitGame();
		}

#endregion
	}
}