using JusticeFramework.Components;
using JusticeFramework.Events;
using JusticeFramework.Interfaces;
using JusticeFramework.Managers;
using JusticeFramework.Core.Extensions;
using System;
using UnityEngine;

namespace JusticeFramework.Controllers {
	[Serializable]
	public class InteractionController : MonoBehaviour, IInteractionController {
		private const float InteractionDistance = 2.5f;

		public event OnWorldTargetChanged onInteractionTargetChanged;

		[SerializeField]
		private Transform mainCamera;

		[SerializeField]
		private WorldObject lookingAt;

		[SerializeField]
		private Transform lookingAtTransform;

        [SerializeField]
        private KeyCode attackKeyCode = KeyCode.Mouse0;

        [SerializeField]
        private KeyCode attackSecondaryKeyCode = KeyCode.Mouse1;

        [SerializeField]
        private KeyCode interactKeyCode = KeyCode.E;

        private bool ignoreNextKey;
		private RaycastHit raycastHit;

		/// <summary>
		/// The current target the user is looking at
		/// </summary>
		public WorldObject CurrentTarget {
			get { return lookingAt; }
		}

		/// <summary>
		/// Initializes the object when first enabled
		/// </summary>
		private void Awake() {
			mainCamera = Camera.main.transform;
			lookingAt = null;
		}

		/// <summary>
		/// Update every frame called after the physics engine has updated
		/// </summary>
		private void Update() {
            if (GameManager.IsPaused) {
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
			if (mainCamera.RaycastFromCenterForward(out raycastHit, InteractionDistance)) {
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
                onInteractionTargetChanged?.Invoke(lookingAt);
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
                onInteractionTargetChanged?.Invoke(lookingAt);
			}
		}

		/// <summary>
		/// Handles the interactions that may come from the user
		/// </summary>
		private void HandleInteraction() {
			// If the user is trying to interact
			if (Input.GetKeyDown(interactKeyCode)) {
				// Activate the object if able
				Logic.Action a = lookingAt?.Activate(GameManager.GetPlayer());
                a?.Execute(GameManager.GetPlayer());
			} else {
                // Else if the user is trying to attack
                //HandleAttack(GameManager.Player, attackKeyCode, attackSecondaryKeyCode);
			}
		}

		/// <summary>
		/// Handles the attacking flow on the given actor
		/// </summary>
		/*private void HandleAttack(IActor actor, KeyCode primaryKey, KeyCode secondaryKey) {
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
		}*/
	}
}
