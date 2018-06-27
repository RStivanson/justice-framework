using System;
using UnityEngine;

namespace JusticeFramework.Core.Models {
    /// <summary>
    /// Data class for weapon ammo related objects
    /// </summary>
    [Serializable]
    public class AmmoModel : ItemModel {
        /// <summary>
        /// The type of ammo that this applys to
        /// </summary>
        public EAmmoType ammoType;

        /// <summary>
        /// The amount of damage this ammo can cause
        /// </summary>
        public float damage;
    }
}
