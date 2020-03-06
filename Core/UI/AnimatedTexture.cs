using System;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.Core.UI {
	/// <summary>
	/// Script to play a series of images on a Image UI component
	/// </summary>
	[Serializable]
	[RequireComponent(typeof(Image))]
	public class AnimatedTexture : MonoBehaviour {
		/// <summary>
		/// The amount of images to show per second
		/// </summary>
		[SerializeField]
		private int framesPerSecond = 24;
		
		/// <summary>
		/// A list of images to show, in order
		/// </summary>
		[SerializeField]
		private Sprite[] sprites;
		
		/// <summary>
		/// The Image component whose sprite should be used to display the sequence
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private Image image;
		
		/// <summary>
		/// Initializes the animation script
		/// </summary>
		private void Awake() {
			image = GetComponent<Image>();
		}

		/// <summary>
		/// Updates the animation, called once per frame.
		/// </summary>
		private void Update() {
			// If the sprites array is null or empty, do nothing
			if (sprites == null || sprites.Length == 0) {
				return;
			}
			
			// Calculate where we should be at in the animation sequence at this point in time
			int index = (int)((Time.time * framesPerSecond) % sprites.Length);
			
			// Update the image sprite
			image.sprite = sprites[index];
		}
	}
}