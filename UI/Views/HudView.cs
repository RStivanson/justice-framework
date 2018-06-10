using JusticeFramework.Components;
using JusticeFramework.Interfaces;
using System;
using UnityEngine;

namespace JusticeFramework.UI.Views {
    using Components;
    using JusticeFramework.Core.Interfaces;
    using JusticeFramework.Core.UI;

    [Serializable]
	public class HudView : Window {
		[SerializeField]
		[HideInInspector]
		private IInteractionController interactionController;

		private WorldObject currentTarget;
		
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

			interactionController = player.GetComponent<IInteractionController>();

			interactionController.OnInteractionTargetChanged += crosshair.OnInteractionTargetChanged;
			interactionController.OnInteractionTargetChanged += OnInteractionTargetChanged;
			OnInteractionTargetChanged(interactionController.CurrentTarget);
		}
		
		/// <summary>
		/// Sets the controller whose targets we should monitor
		/// </summary>
		/// <param name="controller">The controller to get updates from</param>
		public void OnInteractionTargetChanged(IWorldObject reference) {
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