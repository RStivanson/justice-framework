using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Model class for all activators such as levers, buttons, or pressure plates.
	/// </summary>
	[Serializable]
	public class ActivatorModel : WorldObjectModel {
		/// <summary>
		/// Sound clip to be played upon activation
		/// </summary>
		public AudioClip activationSound = null;
	}
}