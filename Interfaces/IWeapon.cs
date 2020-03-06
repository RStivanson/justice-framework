using UnityEngine;

namespace JusticeFramework.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for weapons
	/// </summary>
	public interface IWeapon : IEquippable {
		/// <summary>
		/// The type of weapon
		/// </summary>
		EWeaponType WeaponType { get; }
		
		/// <summary>
		/// The amount of damage dealt by this weapon
		/// </summary>
		int Damage { get; }

        /// <summary>
        /// The transform that the equipped actors offhand should target
        /// </summary>
        Transform OffhandIkTarget { get; }

        /// <summary>
        /// An override controller used for play FP animations
        /// </summary>
        AnimatorOverrideController FPOverrideController { get; }

        /// <summary>
        /// An override controller used for play TP animations
        /// </summary>
        AnimatorOverrideController TPOverrideController { get; }

        /// <summary>
        /// The firing motion used by this weapon
        /// </summary>
        EWeaponFireType FireType { get; }

        /// <summary>
        /// The type of ammo that this weapon can shoot
        /// </summary>
        EAmmoType AcceptedAmmo { get; }

        /// <summary>
        /// Determines if this weapon can fire right now
        /// </summary>
        /// <returns>Returns true if the weapon is ready to fire, false otherwise</returns>
        bool CanFire();

        /// <summary>
        /// Begins the firing motion of the weapon
        /// </summary>
        /// <param name="ammoSupply">The container holding the inventory supply</param>
        void StartFire(IContainer ammoSupply = null);

        /// <summary>
        /// Updates the firing motion of the weapon
        /// </summary>
        /// <param name="ammoSupply">The container holding the inventory supply</param>
        /// <return
        EAttackStatus UpdateFire(IContainer ammoSupply = null);

        /// <summary>
        /// Cleans up and ends the firing motion of the weapon
        /// </summary>
        /// <param name="ammoSupply">The container holding the inventory supply</param>
        void EndFire(Transform origin, IContainer ammoSupply = null);
    }
}