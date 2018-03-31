using System;
using JusticeFramework.Components;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.UI.Views {
	using Components;
	
	[Serializable]
	public class HudView : Window {
		[SerializeField]
		[HideInInspector]
		private InteractionController interactionController;

		private Reference currentTarget;
		
		[SerializeField]
		private HealthBar playerHealthBar;
		
		[SerializeField]
		private HealthBar targetHealthBar;
		
		[SerializeField]
		private Compass compass;
		
		[SerializeField]
		private Crosshair crosshair;
		
		public void SetPlayer(Actor player) {
			playerHealthBar.Monitor(player);
			compass.SetRelativeTo(player.transform);

			interactionController = player.GetComponent<InteractionController>();

			interactionController.OnInteractionTargetChanged += crosshair.OnInteractionTargetChanged;
			interactionController.OnInteractionTargetChanged += OnInteractionTargetChanged;
			OnInteractionTargetChanged(interactionController.Current);
		}
		
		/// <summary>
		/// Sets the controller whose targets we should monitor
		/// </summary>
		/// <param name="controller">The controller to get updates from</param>
		public void OnInteractionTargetChanged(Reference reference) {
			if (reference is IDamageable) {
				targetHealthBar.Monitor((IDamageable)reference);
				targetHealthBar.Show();
			} else {
				targetHealthBar.Monitor(null);
				targetHealthBar.Hide();
			}
		}

		protected override void OnShow() {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			targetHealthBar.Hide();
		}

		protected override void OnHide() {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		protected override void OnClose() {
			playerHealthBar.Close();
			targetHealthBar.Close();
			crosshair.Close();
			compass.Close();
			
			interactionController.OnInteractionTargetChanged -= OnInteractionTargetChanged;
		}
	}
}