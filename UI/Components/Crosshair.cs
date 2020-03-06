using JusticeFramework.Components;
using JusticeFramework.Core.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Components {
    [Serializable]
	public class Crosshair : Window {
		[SerializeField]
		private Image crosshair;

		[SerializeField]
		private Text text;
		
		[SerializeField]
		private Sprite speakCrosshair;
		
		[SerializeField]
		private Sprite activateCrosshair;
		
		[SerializeField]
		private Sprite lootCrosshair;

		[SerializeField]
		[HideInInspector]
		private WorldObject current;
		
		/// <summary>
		/// Event called when the interaction controller changes which object it is looking at
		/// </summary>
		/// <param name="newTarget">The new object being looked at</param>
		public void OnInteractionTargetChanged(WorldObject newTarget) {
			// If this is the same object we are already looking at, do nothing
			if (ReferenceEquals(newTarget, current)) {
				return;
			}
			
			// Switch the current monitored reference to the new one
			SwitchCurrentTarget(newTarget);
			
			// If the current target is not null
			if (current != null) {
				// Update the crosshair values
				SetCrosshair(current.DisplayName, current.InteractionType);
			} else {
				// Else reset the crosshair to have no interaction information
				ResetCrosshair();
			}
		}

		/// <summary>
		/// Event called when the state of the reference object changed
		/// </summary>
		/// <param name="changed">The reference object that changed</param>
		private void OnReferenceStateChanged(WorldObject changed) {
			// If we somehow are getting updates for a different object, unsubscribe and return
			if (!ReferenceEquals(current, changed)) {
				changed.onStateChanged -= OnReferenceStateChanged;
				return;
			}

            // If the current target is not null
            if (current != null) {
                SetCrosshair(current.DisplayName, current.InteractionType);
			} else {
				// Else reset the crosshair to have no interaction information
				ResetCrosshair();
			}
		}

		/// <summary>
		/// Switches the current reference to the new reference and handles all event registrations
		/// </summary>
		/// <param name="newReference">The new reference to monitor</param>
		private void SwitchCurrentTarget(WorldObject newReference) {
			// Deregister from the old reference if it is not null
			if (current != null) {
				current.onStateChanged -= OnReferenceStateChanged;
			}

			// Assign the new reference
			current = newReference;

			// Register for state changed events on the new reference if it is not null
			if (current != null) {
				current.onStateChanged += OnReferenceStateChanged;
			}
		}

		/// <summary>
		/// Sets the information on the crosshair
		/// </summary>
		/// <param name="displayName">The name to display on the crosshair</param>
		/// <param name="interactionType">The type of interaction to display on the crosshair</param>
		private void SetCrosshair(string displayName, EInteractionType interactionType) {
			if (interactionType != EInteractionType.None) {
				text.text = $"{displayName}\n{interactionType}";
			} else {
				text.text = string.Empty;
			}

			UpdateCrosshairImage(interactionType);
		}

		/// <summary>
		/// Sets the information on the crosshair to the default values
		/// </summary>
		private void ResetCrosshair() {
			SetCrosshair(string.Empty, EInteractionType.None);
		}
		
		/// <summary>
		/// Sets the crosshair image according to the interaction type
		/// </summary>
		/// <param name="interactionType">The type of interaction that would take place</param>
		private void UpdateCrosshairImage(EInteractionType interactionType) {
			switch (interactionType) {
				case EInteractionType.Activate:
				case EInteractionType.Close:
				case EInteractionType.Open:
				case EInteractionType.Use:
					crosshair.overrideSprite = activateCrosshair;
					break;
				case EInteractionType.Talk:
					crosshair.overrideSprite = speakCrosshair;
					break;
				case EInteractionType.Loot:
				case EInteractionType.Take:
				case EInteractionType.Read:
					crosshair.overrideSprite = lootCrosshair;
					break;
				default:
					crosshair.overrideSprite = null;
					break;
			}
		}

		protected override void OnClose() {
			if (current != null) {
				current.onStateChanged -= OnReferenceStateChanged;
			}
		}
	}
}