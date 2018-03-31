using System;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Model class for all armor pieces.
	/// </summary>
	[Serializable]
	public class ArmorModel : EquippableModel {
		/// <summary>
		/// The incoming damage modifier
		/// </summary>
		public int armorRating = 0;
	}
}