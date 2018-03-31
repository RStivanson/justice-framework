using System;
using JusticeFramework.Data;
using UnityEngine;

namespace JusticeFramework.Controllers {
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

		[Header("Movement")]
		[SerializeField]
		private Motor motor;

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
		
		public bool IsSliding {
			get { return sliding; }
		}
		
		private void Start() {
			animator = GetComponent<Animator>();
			characterController = GetComponent<CharacterController>();

			motor = new Motor();

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

#region Movment
		
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
					motor.State = EMotorState.JUMPING;
				}
			}

			if (velocity.y < -terminalVelocityY) {
				velocity.y = -terminalVelocityY;
			}
		}

		private Vector3 UpdateMovement() {
			Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, UnityEngine.Input.GetAxis("Vertical"));

			motor.UpdateState(moveDirection, Input.GetKey(KeyCode.LeftShift), UnityEngine.Input.GetKeyDown(KeyCode.LeftAlt), !characterController.isGrounded);

			switch (motor.State) {
				case EMotorState.WALKING:
					animator.SetBool("Walking", true);
					break;
				case EMotorState.IDLE:
					animator.SetBool("Walking", false);
					break;
			}

			moveDirection = transform.TransformDirection(moveDirection);

			moveDirection *= motor.GetCurrentSpeed();

			return moveDirection;
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