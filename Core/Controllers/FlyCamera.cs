using JusticeFramework.Core.Models.Settings;
using System;
using UnityEngine;

namespace JusticeFramework.Core.Controllers {
    [Serializable]
	public class FlyCamera : MonoBehaviour {
		private const int RotateMouseButton = 1;
		private const int DragMoveMouseButton = 2;
		
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

			if (Input.GetMouseButton(DragMoveMouseButton)) {
				xMoveDelta = -Input.GetAxis(SystemConstants.InputMouseX) * moveSpeed * Time.deltaTime;
				yMoveDelta = -Input.GetAxis(SystemConstants.InputMouseY) * moveSpeed * Time.deltaTime;
				
				transform.position += transform.right * xMoveDelta;
				transform.position += transform.up * yMoveDelta;
			} else if (Input.GetMouseButton(RotateMouseButton)) {
 				x += Input.GetAxis(SystemConstants.InputMouseX) * rotateSpeed * Time.deltaTime;
				y -= Input.GetAxis(SystemConstants.InputMouseY) * rotateSpeed * Time.deltaTime;

				Quaternion rotation = Quaternion.Euler(y, x, 0);

				transform.rotation = rotation;
			}

			transform.position += transform.forward * zoomDelta;
		}
	}
}