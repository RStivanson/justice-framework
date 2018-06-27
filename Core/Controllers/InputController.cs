using System;
using JusticeFramework.Core.Managers;
using UnityEngine;

namespace JusticeFramework.Core.Controllers {
	[Serializable]
	[DisallowMultipleComponent()]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Animator))]
	public class InputController : Controller {
		[SerializeField]
		[HideInInspector]
		private CharacterController characterController = null;

		[SerializeField]
		private Vector3 velocity = Vector3.zero;
		
		[SerializeField]
		[HideInInspector]
		private Animator animator;

		[Header("Camera")]
		[SerializeField]
		private new Transform camera = null;

		[SerializeField]
		[Range(0, 10)]
		private float XSensitivity = 3.5f;

		[SerializeField]
		[Range(0, 10)]
		private float YSensitivity = 3.0f;

		[SerializeField]
		private bool clampVerticalRotation = true;
		
		[SerializeField]
		private float maxVerticalLook = 80.0f;
		
		[SerializeField]
		private float minVerticalLook = -80.0f;

		[SerializeField]
		private float terminalVelocityY = 35;
		
		private float m_LookAngle = 0;       // The rig's y axis rotation.
		private float m_TiltAngle = 0;       // The pivot's x axis rotation.
		private Quaternion m_CharacterTargetRot;
		private Quaternion m_CameraTargetRot;
		private bool removeCameraOnDestroy = false;

		[Header("Sliding")]
		[SerializeField]
		private Vector3 slideDirection;

		[SerializeField]
		private bool sliding;

		[SerializeField]
		private float slideSpeed = 12f;

		[SerializeField]
		private float minSlopeAngle = 60.0f;

		[SerializeField]
		private float maxSlopeAngle = 89.5f;

        [Header("Movement")]
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

        public const float BackwardsPenalty = 0.85f;
        public const float InAirPenalty = 0.85f;

        private float walkSpeed = 4.0f;
        private float runSpeed = 8.0f;
        private float crouchSpeed = 2.1f;

        [SerializeField]
        public float walkSpeedMod = 1.0f;

        [SerializeField]
        public float runSpeedMod = 1.0f;

        [SerializeField]
        public float crouchSpeedMod = 1.0f;

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

        public bool IsSliding {
			get { return sliding; }
		}
		
		private void Start() {
			animator = GetComponent<Animator>();
			characterController = GetComponent<CharacterController>();

			GameManager.Instance.OnGamePause += OnGamePause;
		}

		private void OnDestroy() {
			Destroy(gameObject.GetComponent<CharacterController>());

			GameManager.Instance.OnGamePause -= OnGamePause;

			if (removeCameraOnDestroy && camera != null) {
				Destroy(camera.GetComponent<Camera>());
			}
		}
		
		private void Update() {
			HandleCameraInput();

			HandleMovementInput();
		}

		private void OnGamePause(bool isPaused) {
			enabled = !isPaused;
		}

		private void HandleCameraInput() {
			if (camera == null) {
				return;
			}

			float yRot = Input.GetAxis("Mouse X") * XSensitivity;
			float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

			m_LookAngle += yRot;
			m_TiltAngle -= xRot;

			if (clampVerticalRotation) {
				m_TiltAngle = Mathf.Clamp(m_TiltAngle, minVerticalLook, maxVerticalLook);
			}

			m_CharacterTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);
			m_CameraTargetRot = Quaternion.Euler(m_TiltAngle, 0, 0);

			camera.localRotation = m_CameraTargetRot;
			transform.localRotation = m_CharacterTargetRot;
		}

		private void HandleMovementInput() {
			if (characterController != null) {
				UpdateVelocity(UpdateMovement());

				characterController.Move(velocity * Time.deltaTime);
			}
		}

		private void OnControllerColliderHit(ControllerColliderHit hit) {
			if ((characterController.collisionFlags & CollisionFlags.Above) != 0 && velocity.y > 0) {
				velocity.y = -Time.deltaTime;
			}

			Rigidbody rigidbody = hit.gameObject.GetComponent<Rigidbody>();

			if (hit.moveDirection.y < -0.3 && rigidbody != null && !rigidbody.isKinematic) {
				rigidbody.velocity += velocity;
			} else {
				UpdateSlideState(hit.normal);
			}
		}

#region Movement
		
		private void UpdateVelocity(Vector3 newDirection) {
			// Set the new direction we are trying to move towards
			velocity.x = newDirection.x;
			velocity.z = newDirection.z;
			
			if (!characterController.isGrounded) {
				sliding = false;
				slideDirection = Vector3.zero;
				
				// Apply gravity
				velocity.y += Physics.gravity.y * Time.deltaTime;
			} else {
				if (sliding) {
					// Update our velocity to reflect us being forced to slide
					velocity += slideDirection * Time.deltaTime;
				}
				
				velocity.y = -characterController.stepOffset;// / Time.deltaTime;

				if (!sliding && Input.GetKeyDown(KeyCode.Space)) {
					velocity.y = 5f;
					State = EMotorState.JUMPING;
				}
			}

			if (velocity.y < -terminalVelocityY) {
				velocity.y = -terminalVelocityY;
			}
		}

		private Vector3 UpdateMovement() {
			Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, UnityEngine.Input.GetAxis("Vertical"));

			UpdateState(moveDirection, Input.GetKey(KeyCode.LeftShift), Input.GetKeyDown(KeyCode.LeftAlt), !characterController.isGrounded);

			switch (State) {
				case EMotorState.WALKING:
					animator.SetBool("Walking", true);
					break;
				case EMotorState.IDLE:
					animator.SetBool("Walking", false);
					break;
			}

			moveDirection = transform.TransformDirection(moveDirection);

			moveDirection *= GetCurrentSpeed();

			return moveDirection;
		}

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

        #region Slope Sliding

        private void UpdateSlideState(Vector3 normal) {
			float angle = Vector3.Angle(normal, Vector3.up);

			if (angle >= minSlopeAngle && angle <= maxSlopeAngle) {
				sliding = true;

				Vector3 u = new Vector3(normal.x, -normal.y, normal.z);
				Vector3.OrthoNormalize (ref normal, ref u);
				slideDirection = u * -Physics.gravity.y * slideSpeed * (angle / minSlopeAngle);
			} else {
				sliding = false;
				slideDirection = Vector3.zero;
			}
		}
		
#endregion
	}
}