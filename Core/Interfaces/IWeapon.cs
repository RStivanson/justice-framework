using UnityEngine;

namespace JusticeFramework.Core.Interfaces {
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
        /// Animation used when the weapon is used to attack
        /// </summary>
        AnimationClip AttackAnimation { get; }

        /// <summary>
        /// The firing motion used by this weapon
        /// </summary>
        EWeaponFireType FireType { get; }

        /// <summary>
        /// The type of ammo that this weapon can shoot
        /// </summary>
        EAmmoType AcceptedAmmo { get; }

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
        void UpdateFire(IContainer ammoSupply = null);

        /// <summary>
        /// Cleans up and ends the firing motion of the weapon
        /// </summary>
        /// <param name="ammoSupply">The container holding the inventory supply</param>
        void EndFire(Transform origin, IContainer ammoSupply = null);
    }
}