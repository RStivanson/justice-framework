using System;
using UnityEngine;

namespace JusticeFramework.Core.Controllers {
	[Serializable]
	public class FlyCamera : MonoBehaviour {
		private const int UNITY_ROTATE_MOUSE_BUTTON = 1;
		private const int UNITY_DRAG_MOVE_MOUSE_BUTTON = 2;
		
		[SerializeField]
		private float zoomSpeed = 75.0f;
		
		[SerializeField]
		private int zoomModifier = 5;
		
		[SerializeField]
		private float moveSpeed = 30.0f;
		
		[SerializeField]
		private float rotateSpeed = 160.0f;
 
		private float x;
		private float y;
		private float zoomDelta;
		private float xMoveDelta, yMoveDelta;
 
		private void Awake ()  {
			x = transform.eulerAngles.y;
			y = transform.eulerAngles.x;
		}
 
		private void Update()  {
			zoomDelta = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? zoomModifier : 1);

			if (Input.GetMouseButton(UNITY_DRAG_MOVE_MOUSE_BUTTON)) {
				xMoveDelta = -Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
				yMoveDelta = -Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
				
				transform.position += transform.right * xMoveDelta;
				transform.position += transform.up * yMoveDelta;
			} else if (Input.GetMouseButton(UNITY_ROTATE_MOUSE_BUTTON)) {
 				x += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
				y -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

				Quaternion rotation = Quaternion.Euler(y, x, 0);

				transform.rotation = rotation;
			}

			transform.position += transform.forward * zoomDelta;
		}
	}
}