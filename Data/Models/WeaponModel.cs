using System;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Data class for all weapons
	/// </summary>
	[Serializable]
	public class WeaponModel : EquippableModel {
		/// <summary>
		/// Determines the weapon type and how it is used.
		/// </summary>
		public EWeaponType weaponType = EWeaponType.OneHanded;

		/// <summary>
		/// The damage output of the weapon
		/// </summary>
		public int damage = 0;
	}
}