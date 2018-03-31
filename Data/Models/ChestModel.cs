using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Model class for all containers such as chests or lockers.
	/// </summary>
	[Serializable]
	public class ChestModel : WorldObjectModel {
		/// <summary>
		/// Sound clip to be played when opened
		/// </summary>
		public AudioClip openSound = null;
	}
}