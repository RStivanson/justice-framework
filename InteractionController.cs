using System;
using JusticeFramework.Components;
using JusticeFramework.Data.Events;
using JusticeFramework.Interfaces;
using JusticeFramework.Utility.Extensions;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JusticeFramework {
	public delegate void OnReferenceTargetChanged(Reference newTarget);

	[Serializable]
	public class InteractionController : MonoBehaviour {
		private const float INTERACTION_DISTANCE = 2.5f;

		[SerializeField]
		private Transform mainCamera;

		[SerializeField]
		private Reference lookingAt;

		[SerializeField]
		private Transform lookingAtTransform;
		
		private RaycastHit raycastHit;

		public event OnReferenceTargetChanged OnInteractionTargetChanged;
		
		/// <summary>
		/// The current target the user is looking at
		/// </summary>
		public Reference Current {
			get { return lookingAt; }
		}

		/// <summary>
		/// Initializes the object when first enabled
		/// </summary>
		private void Awake() {
			mainCamera = Camera.main.transform;
			lookingAt = null;

			GameManager.Instance.OnGamePause += OnGamePaused;
		}

		/// <summary>
		/// Cleans up if this object is destroyed
		/// </summary>
		private void OnDestroy() {
			GameManager.Instance.OnGamePause -= OnGamePaused;
		}

		/// <summary>
		/// Update every frame called after the physics engine has updated
		/// </summary>
		private void LateUpdate() {
			if (EventSystem.current.IsPointerOverGameObject()) {
				return;
			}

			// Update the current target and check for interactions
			HandleLookAtUpdate();
			HandleInteraction();
		}

		/// <summary>
		/// Updates the current target being looked at by the user
		/// </summary>
		private void HandleLookAtUpdate() {
			// If the player is looking at something
			if (mainCamera.RaycastFromCenterForward(out raycastHit, INTERACTION_DISTANCE)) {
				// If the object the player is looking at is the same as the current target
				if (ReferenceEquals(lookingAtTransform, raycastHit.transform)) {
					return;
				}

				lookingAtTransform = raycastHit.transform;
				Reference reference = lookingAtTransform?.GetComponentInCurrentOrParent<Reference>();
				
				// If the object has a reference object attached to it
				if (reference != null) {
					// Update the current target to be the new object and get the Reference script
					lookingAt = reference;
				} else {
					// Update the target script to be null
					lookingAt = null;
				}
				
				// Inform anyone listening
				OnInteractionTargetChanged?.Invoke(lookingAt);
			} else {
				// If the current target is already null, do nothing
				if (lookingAtTransform == null) {
					return;
				}
				
				// Update the targets to be null
				lookingAtTransform = null;
				lookingAt = null;
				
				// Inform anyone listening
				OnInteractionTargetChanged?.Invoke(lookingAt);
			}
		}

		private void ClearTarget() {
			
		}

		/// <summary>
		/// Handles the interactions that may come from the user
		/// </summary>
		private void HandleInteraction() {
			// If the user is trying to interact
			if (Input.GetKeyDown(KeyCode.E)) {
				// Activate the object if able
				lookingAt?.Activate(this, new ActivateEventArgs(null, GameManager.Player));
			} else if (Input.GetMouseButtonDown(0)) { // Else if the user is trying to attack
				// If the play is not in combat, enter combat
				if (!GameManager.Player.IsInCombat) {
					GameManager.Player.EnterCombat();
				} else {
					// Else try to swing
					Attack();
				}
			}
		}

		/// <summary>
		/// Handles causing the player to attack
		/// </summary>
		private void Attack() {
			// If the player isn't currently swinging
			if (GameManager.Player.IsRightSwinging()) {
				return;
			}

			// Make the player attack
			GameManager.Player.Swing();
			(lookingAt as IDamageable)?.Damage(GameManager.Player, GameManager.Player.TotalDamage);
		}

#region Event Callbacks

		/// <summary>
		/// Handles disabling the component when the game is paused
		/// </summary>
		/// <param name="isPaused">Flag inidicating the pause status of the game</param>
		private void OnGamePaused(bool isPaused) {
			enabled = !isPaused;
		}

#endregion
	}
}
