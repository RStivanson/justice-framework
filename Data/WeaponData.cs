using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Data class for all weapons
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Weapon Data")]
    public class WeaponData : ItemData {
        /// <summary>
        /// The object that should be spawned when this item is equipped
        /// </summary>
        [SerializeField]
        private GameObject equipmentPrefab;

        /// <summary>
        /// The slot where the armor should be equipped
        /// </summary>
        [SerializeField]
        private EEquipSlot equipSlot = EEquipSlot.Head;

        /// <summary>
        /// Sound clip to be played upon equip
        /// </summary>
        [SerializeField]
        private AudioClip equipSound = null;

        /// <summary>
        /// Determines the weapon type and how it is used.
        /// </summary>
        [SerializeField]
        private EWeaponType weaponType = EWeaponType.OneHanded;

        /// <summary>
        /// The damage output of the weapon
        /// </summary>
        [SerializeField]
        private int damage = 0;

        /// <summary>
        /// Animation played when the weapon is used to attack
        /// </summary>
        [SerializeField]
        private AnimationClip attackAnimation;

        /// <summary>
        /// An override controller used for play FP animations
        /// </summary>
        [SerializeField]
        private AnimatorOverrideController fpOverrideController;

        /// <summary>
        /// An override controller used for play TP animations
        /// </summary>
        [SerializeField]
        private AnimatorOverrideController tpOverrideController;

        /// <summary>
        /// The firing motion used by this weapon
        /// </summary>
        [SerializeField]
        private EWeaponFireType fireType;

        /// <summary>
        /// Determines the weapon ammo accepted for linear and projectile weapons
        /// </summary>
        [SerializeField]
        private EAmmoType acceptedAmmo = EAmmoType.Arrow;

        /// <summary>
        /// Gets the prefab spawned when this weapon is equipped.
        /// </summary>
        public GameObject EquipmentPrefab {
            get { return equipmentPrefab; }
        }

        /// <summary>
        /// Gets the slot where this weapon is equipped.
        /// </summary>
        public EEquipSlot EquipSlot {
            get { return equipSlot; }
        }

        /// <summary>
        /// Gets the type of this weapon.
        /// </summary>
        public EWeaponType WeaponType {
            get { return weaponType; }
        }

        /// <summary>
        /// Gets the base damage of this weapon.
        /// </summary>
        public int Damage {
            get { return damage; }
        }

        /// <summary>
        /// Gets the attack animation for this weapon.
        /// </summary>
        public AnimationClip AttackAnimation {
            get { return attackAnimation; }
        }

        /// <summary>
        /// Gets the first-person animation override controller.
        /// </summary>
        public AnimatorOverrideController FpOverrideController {
            get { return fpOverrideController; }
        }

        /// <summary>
        /// Gets the third-person animation override controller.
        /// </summary>
        public AnimatorOverrideController TpOverrideController {
            get { return tpOverrideController; }
        }

        /// <summary>
        /// Gets the firing type of this weapon.
        /// </summary>
        public EWeaponFireType FireType {
            get { return fireType; }
        }

        /// <summary>
        /// Gets the ammo that is used by this weapon.
        /// </summary>
        public EAmmoType AcceptedAmmo {
            get { return acceptedAmmo; }
        }
    }
}