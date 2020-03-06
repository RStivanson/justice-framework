using System;

namespace JusticeFramework {
	/// <summary>
	/// Type of weapon
	/// </summary>
	[Flags]
	public enum EWeaponType {
		/// <summary>
		/// Single handed weapon
		/// </summary>
		OneHanded = 1,
		
		/// <summary>
		/// Two handed weapon
		/// </summary>
		TwoHanded = 2,
	}
}