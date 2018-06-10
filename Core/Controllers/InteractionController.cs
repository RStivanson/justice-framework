using JusticeFramework.Components;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using JusticeFramework.Utility.Extensions;
using System;
using UnityEngine;

namespace JusticeFramework.Core.Controllers {
    public delegate void OnWorldTargetChanged(WorldObject newTarget);

	[Serializable]
	public class InteractionController : MonoBehaviour {
		private const float INTERACTION_DISTANCE = 2.5f;

		[SerializeField]
		private Transform mainCamera;

		[SerializeField]
		private WorldObject lookingAt;

		[SerializeField]
		private Transform lookingAtTransform;

        private KeyCode attackKeyCode = KeyCode.Mouse0;
        private KeyCode attackSecondaryKeyCode = KeyCode.Mouse1;

        private KeyCode interactKeyCode = KeyCode.E;

        private bool ignoreNextKey;

		private RaycastHit raycastHit;

		public event OnWorldTargetChanged OnInteractionTargetChanged;
		
		/// <summary>
		/// The current target the user is looking at
		/// </summary>
		public WorldObject Current {
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
		private void Update() {
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
				WorldObject reference = lookingAtTransform?.GetComponentInCurrentOrParent<WorldObject>() ?? null;
				
				// If the object has a reference object attached to it
				if (reference != null) {
					// Update the current target to be the new object and get the Reference script
					lookingAt = reference;
				} else {
					// Update the the references to be null
					lookingAt = null;
                    lookingAtTransform = null;
				}

                // Inform anyone listening
                OnInteractionTargetChanged?.Invoke(lookingAt);
			} else {
                // If the current target is already null, do nothing
                // We use ReferenceEquals to check if the variable is actually null or if we still have an object pointer
                if (ReferenceEquals(lookingAtTransform, null)) {
					return;
				}
				
				// Update the targets to be null
				lookingAtTransform = null;
				lookingAt = null;

                // Inform anyone listening
                OnInteractionTargetChanged?.Invoke(lookingAt);
			}
		}

		/// <summary>
		/// Handles the interactions that may come from the user
		/// </summary>
		private void HandleInteraction() {
			// If the user is trying to interact
			if (Input.GetKeyDown(interactKeyCode)) {
				// Activate the object if able
				lookingAt?.Activate(this, new ActivateEventArgs(null, GameManager.Player));
			} else {
                // Else if the user is trying to attack
                HandleAttack(GameManager.Player, attackKeyCode, attackSecondaryKeyCode);
			}
		}

		/// <summary>
		/// Handles the attacking flow on the given actor
		/// </summary>
		private void HandleAttack(IActor actor, KeyCode primaryKey, KeyCode secondaryKey) {
            if (actor.IsInCombat) {
                if (Input.GetKeyDown(secondaryKey)) {
                    actor.ExitCombat();
                } else if (Input.GetKeyDown(attackKeyCode)) {
                    actor.BeginAttack();
                } else if (Input.GetKeyUp(attackKeyCode)) {
                    if (ignoreNextKey) {
                        ignoreNextKey = false;
                    } else {
                        actor.EndAttack();
                    }
                } else if (!ignoreNextKey) {
                    actor.UpdateAttack();
                }
            } else {
                if (Input.GetKeyDown(attackKeyCode)) {
                    actor.EnterCombat();
                    ignoreNextKey = true;
                }
            }
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
