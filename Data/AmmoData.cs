using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Data class for weapon ammo related objects
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Ammo Data")]
    public class AmmoData : ItemData {
        /// <summary>
        /// The amount of damage this ammo can cause.
        /// </summary>
        [SerializeField]
        private float damage;

        /// <summary>
        /// Gets the amount of damage this ammo can cause
        /// </summary>
        public float Damage {
            get { return damage; }
        }
    }
}
