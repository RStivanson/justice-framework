using System;
using JusticeFramework.Data;
using UnityEngine;

namespace JusticeFramework.Controllers {
	[Serializable]
	public class Motor {
		#region Variables and Properties
		// State
		private bool isMovingBackwards;
		private bool isInAir;

		[SerializeField]
		private bool canMove = true;

		[SerializeField]
		private bool canRun = true;

		[SerializeField]
		private bool canCrouch = true;

		[SerializeField]
		private bool canMoveInAir = true;

		// Speeds and Modifiers
		public const float BackwardsPenalty = 0.85f;
		public const float InAirPenalty = 0.85f;

		private float walkSpeed = 4.0f;
		private float runSpeed = 8.0f;
		private float crouchSpeed = 2.1f;

		public float walkSpeedMod = 1.0f;
		public float runSpeedMod = 1.0f;
		public float crouchSpeedMod = 1.0f;

		// Properties
		public EMotorState State {
			get; set;
		}
		
		public float WalkSpeed {
			get { return walkSpeed; }
			set { walkSpeed = value; }
		}

		public float WalkSpeedModifier {
			get { return walkSpeedMod; }
			set { walkSpeedMod = value; }
		}

		public float RunSpeed {
			get { return runSpeed; }
			set { runSpeed = value; }
		}

		public float RunSpeedModifer {
			get { return runSpeedMod; }
			set { runSpeedMod = value; }
		}

		public float CrouchSpeed {
			get { return crouchSpeed; }
			set { crouchSpeed = value; }
		}

		public float CrouchSpeedModifier {
			get { return crouchSpeedMod; }
			set { crouchSpeedMod = value; }
		}

		public bool IsMoving { get; set; }
		#endregion

		#region State Machine Methods
		public void UpdateState(Vector3 moveDirection, bool requestingRun, bool requestingCrouch, bool isFalling) {
			IsMoving = moveDirection != Vector3.zero;
			isMovingBackwards = moveDirection.z < 0;
			isInAir = isFalling;
			
			if (!canMove || (moveDirection == Vector3.zero)) {
				State = EMotorState.IDLE;
			} else if (canCrouch && requestingCrouch) {
				if (State == EMotorState.CROUCHING) {
					State = EMotorState.WALKING;
				} else {
					State = EMotorState.CROUCHING;
				}
			} else if (canRun && requestingRun && !isMovingBackwards) {
				State = EMotorState.RUNNING;
			} else if (State == EMotorState.CROUCHING && isFalling) {
				State = EMotorState.WALKING;
			} else {
				if (State != EMotorState.CROUCHING) {
					State = EMotorState.WALKING;
				}
			}
		}

		public float GetCurrentSpeed() {
			float speed;

			switch (State) {
				case EMotorState.WALKING:
					speed = walkSpeed * walkSpeedMod;
					break;
				case EMotorState.RUNNING:
					speed = runSpeed * runSpeedMod;
					break;
				case EMotorState.CROUCHING:
					speed = crouchSpeed * crouchSpeedMod;
					break;
				default:
					speed = 0.0f;
					break;
			}

			if (isMovingBackwards)
				speed *= BackwardsPenalty;

			if (isInAir && canMoveInAir)
				speed *= InAirPenalty;

			return speed;
		}
#endregion
	}
}