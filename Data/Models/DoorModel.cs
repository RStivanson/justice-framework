using System;
using JusticeFramework.Data;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Data class for all doors.
	/// </summary>
	[Serializable]
	public class DoorModel : WorldObjectModel {
		/// <summary>
		/// The type of door behaviour
		/// </summary>
		public EDoorType doorType;
		
		/// <summary>
		/// Sound clip to be played upon activation
		/// </summary>
		public AudioClip activationSound = null;
	}
}