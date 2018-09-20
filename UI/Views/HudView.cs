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

        [SerializeField]
        private NotificationView notifications;
		
		public void SetPlayer(Actor player) {
			playerHealthBar.Monitor(player);
			compass.SetRelativeTo(player.transform);

			interactionController = player.GetComponent<IInteractionController>();

			interactionController.onInteractionTargetChanged += crosshair.OnInteractionTargetChanged;
			interactionController.onInteractionTargetChanged += OnInteractionTargetChanged;
			OnInteractionTargetChanged(interactionController.CurrentTarget);
		}
		
		/// <summary>
		/// Sets the controller whose targets we should monitor
		/// </summary>
		/// <param name="controller">The controller to get updates from</param>
		private void OnInteractionTargetChanged(IWorldObject reference) {
			if (reference is IDamageable) {
				targetHealthBar.Monitor((IDamageable)reference);
				targetHealthBar.Show();
			} else {
				targetHealthBar.Monitor(null);
				targetHealthBar.Hide();
			}
		}

        public void ShowNotification(string notification) {
            notifications.CreateNotification(notification);
        }

		protected override void OnShow() {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

            playerHealthBar.Show();
            //compass.Show();
            crosshair.Show();
            notifications.Show();

            OnInteractionTargetChanged(interactionController?.CurrentTarget);
		}

		protected override void OnHide() {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

            playerHealthBar.Hide();
            targetHealthBar.Hide();
            //compass.Hide();
            crosshair.Hide();
        }

		protected override void OnClose() {
			playerHealthBar.Close();
			targetHealthBar.Close();
			crosshair.Close();
			compass.Close();
            notifications.Close();
			
			interactionController.onInteractionTargetChanged -= OnInteractionTargetChanged;
		}
	}
}