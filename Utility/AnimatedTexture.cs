using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.Utility {
	[RequireComponent(typeof(Image))]
	public class AnimatedTexture : MonoBehaviour {
		public int framesPerSecond = 24;
		public Sprite[] sprites;
		private Image image;

		private void Awake() {
			image = GetComponent<Image>();
		}

		private void Update() {
			if (sprites != null && sprites.Length > 0) {
				int index = (int)((Time.time * framesPerSecond) % sprites.Length);
				image.sprite = sprites[index];
			}
		}
	}
}