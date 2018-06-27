using JusticeFramework.Core.Managers;
using JusticeFramework.Core.UI;
using System;
using UnityEngine;

namespace JusticeFramework.UI.Views {
	[Serializable]
	public class SystemMenuView : Window {
		[SerializeField]
		private Transform buttonContainer;
	
		[SerializeField]
		private GameObject buttonPrefab;
	
#region Button Callbacks
	
		public void ResumeGame() {
			Close();
		}
	
		public void ExitGame() {
			GameManager.ExitGame();
		}
	
#endregion
	}
}