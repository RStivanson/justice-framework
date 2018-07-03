using System;
using UnityEngine;

namespace JusticeFramework.Core.Models {
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

        /// <summary>
        /// Animation played when the weapon is used to attack
        /// </summary>
        public AnimationClip attackAnimation;

        /// <summary>
        /// An override controller used for play FP animations
        /// </summary>
        public AnimatorOverrideController fpOverrideController;

        /// <summary>
        /// An override controller used for play TP animations
        /// </summary>
        public AnimatorOverrideController tpOverrideController;

        /// <summary>
        /// The firing motion used by this weapon
        /// </summary>
        public EWeaponFireType fireType;

        /// <summary>
        /// Determines the weapon ammo accepted for linear and projectile weapons
        /// </summary>
        public EAmmoType acceptedAmmo = EAmmoType.Arrow;
    }
}