using JusticeFramework.Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Components {
	/// <summary>
	/// UI class that rotates a cardinal point image to show the facing direction of the watched object
	/// </summary>
	[Serializable]
	public class Compass : Window {
		/// <summary>
		/// The cardinal point image
		/// </summary>
		[SerializeField]
		private Transform compassImage;
		
		/// <summary>
		/// Number of pixels from one north point to the next (generally, half the width of the image)
		/// </summary>
		[SerializeField]
		private float numberOfPixelsFromNorthToNorth = 512;

		/// <summary>
		/// The transform we are watching
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private Transform relative;
		
		/// <summary>
		/// The starting position of the image
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private Vector3 startPosition;
		
		/// <summary>
		/// Number of pixels per degree (image width / 360)
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private float pixelToDegreeRatio;

		/// <summary>
		/// The recorded last width of the screen
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private float lastScreenWidth;
		
		/// <summary>
		/// Initializes the compass
		/// </summary>
		private void Start() {
			startPosition = compassImage.position;
			pixelToDegreeRatio = numberOfPixelsFromNorthToNorth / 360f;
			lastScreenWidth = Screen.width;
		}
 
		/// <summary>
		/// Updates the location of the compass image
		/// </summary>
		private void Update() {
			// If we are supposed to watch null, do nothing
			if (relative == null) {
				return;
			}
			
			// Update the position of the image
			Vector3 perpendicular = Vector3.Cross(Vector3.forward, relative.forward);
			float direction = -Vector3.Dot(perpendicular, Vector3.up);
			float deltaAngle = Vector3.Angle(Vector3.forward, relative.forward);
			
			// Update the images position;
			compassImage.position = startPosition + new Vector3(deltaAngle * Mathf.Sign(direction) * pixelToDegreeRatio, 0, 0);
		}
		
		
		/// <summary>
		/// Set the transform we are supposed to watch
		/// </summary>
		/// <param name="relativeTo">The transform to watch</param>
		public void SetRelativeTo(Transform relativeTo) {
			relative = relativeTo;
		}
	}
}
