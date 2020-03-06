using JusticeFramework.Interfaces;
using System;
using UnityEngine;

namespace JusticeFramework.Collections {
    /// <summary>
    /// Stores information regarding a piece of equipped gear
    /// </summary>
    [Serializable]
    public class EquippedItem {
        /// <summary>
        /// A reference to item model that is equipped
        /// </summary>
        [SerializeField]
        private IEquippable equippedItem;

        /// <summary>
        /// A reference to the item's parent's animator
        /// </summary>
        [SerializeField]
        private PerspectiveAnimator animator;

        /// <summary>
        /// Gets the item model for the equipped item
        /// </summary>
        public IEquippable EquippedObject {
            get { return equippedItem; }
        }

        /// <summary>
        /// Gets the item's parent's animator
        /// </summary>
        public PerspectiveAnimator Animator {
            get { return animator; }
        }

        /// <summary>
        /// Gets the equipment slot that this item represents
        /// </summary>
        public EEquipSlot EquipSlot {
            get { return equippedItem.EquipSlot; }
        }

        /// <summary>
        /// Gets the GameObject that this equipment represents
        /// </summary>
        public GameObject WorldObject {
             get { return equippedItem.Transform?.gameObject; }
        }

        /// <summary>
        /// Constructs a new equipment item
        /// </summary>
        /// <param name="item">The item to store</param>
        public EquippedItem(IEquippable item, PerspectiveAnimator animator) {
            equippedItem = item;
            this.animator = animator;
        }
    }
}
