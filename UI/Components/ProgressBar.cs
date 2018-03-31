using System;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.UI;
using UnityEngine;

namespace JusticeFramework.UI.Components {
	[Serializable]
	[AddComponentMenu("UGS/UI/Progress Bar")]
	public class ProgressBar : Window, IProgressBar {
		[SerializeField]
		[Range(0, 1)]
		private float value;

		[SerializeField]
		private Transform foreground;
		
		[SerializeField]
		private EGrowthOrigin growthOrigin;

		[SerializeField]
		private EGrowthDirection growthDirection;
		
		public float Value {
			get { return value; }
		}

		/// <summary>
		/// Updates the progress bar inside the Unity Editor.
		/// </summary>
		private void OnValidate() {
			if (foreground == null) {
				return;
			}

			RectTransform rectTransform = foreground.GetComponent<RectTransform>();

			switch (growthOrigin) {
				case EGrowthOrigin.Left:
					rectTransform.pivot = new Vector2(0, 0);
					rectTransform.anchoredPosition = new Vector3(0, 0, -1);
					break;
				case EGrowthOrigin.Right:
					rectTransform.pivot = new Vector2(1, 1);
					rectTransform.anchoredPosition = new Vector3(0.5f, 0.5f, -1);
					break;
				default: // EGrowthOrigin.Center
					rectTransform.pivot = new Vector2(0.5f, 0.5f);
					rectTransform.anchoredPosition = new Vector3(0, 0, -1);
					break;
			}

			SetValue(Value);
		}

		/// <summary>
		/// Sets the value of the progress bar using the given percentage. Clamped between 0 and 1.
		/// </summary>
		/// <param name="percentValue">The percentage of the bar to be filled.</param>
		public void SetValue(float percentValue) {
			value = Mathf.Clamp01(percentValue);

			switch (growthDirection) {
				case EGrowthDirection.Horizontal:
					foreground.localScale = new Vector3(value, 1, 1);
					break;
				default: // EGrowthDirection.Vertical
					foreground.localScale = new Vector3(1, value, 1);
					break;
			}
		}

		/// <summary>
		/// Sets the value of the progress bar by dividing the current value by the maximum value. Does not check for divide by zero. Final value clamped between 0 and 1.
		/// </summary>
		/// <param name="currentValue">The current value relative to the maximum value</param>
		/// <param name="maximumValue">The maximum value of the progress bar</param>
		public void SetValue(float currentValue, float maximumValue) {
			SetValue(currentValue / maximumValue);
		}
	}
}