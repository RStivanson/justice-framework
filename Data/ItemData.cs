using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Base model class for all items.
    /// </summary>
    public abstract class ItemData : ScriptableDataObject, ISceneDataObject, IDisplayable {
        /// <summary>
        /// The name to be displayed in-game.
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
        /// The object that is spawned when this item is placed into the overworld.
        /// </summary>
        [SerializeField]
        private GameObject scenePrefab;

        /// <summary>
        /// The weight of the item. 
        /// </summary>
        [SerializeField]
        private float weight = 0;

        /// <summary>
        /// The value of the item to merchants.
        /// </summary>
        [SerializeField]
        private int value = 0;

        /// <summary>
        /// Determines if the item will stack in an inventory.
        /// </summary>
        [SerializeField]
        private bool isStackable = true;

        /// <summary>
        /// Sound clip to be played upon pickup.
        /// </summary>
        [SerializeField]
        private AudioClip pickupSound = null;

        /// <summary>
        /// Sound clip to be played upon removing the item from an inventory.
        /// </summary>
        [SerializeField]
        private AudioClip dropSound = null;

        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        public string DisplayName {
            get { return displayName; }
        }

        /// <summary>
        /// Gets the prefab that represents this object when it spawned in the scene.
        /// </summary>
        public GameObject ScenePrefab {
            get { return scenePrefab; }
        }

        /// <summary>
        /// Gets the weight of the item. Affects encumberance.
        /// </summary>
        public float Weight {
            get { return weight; }
        }

        /// <summary>
        /// Gets the value of the item.
        /// </summary>
        public int Value {
            get { return value; }
        }

        /// <summary>
        /// Gets if this object is stackable in an inventory.
        /// </summary>
        public bool IsStackable {
            get { return isStackable; }
        }

        /// <summary>
        /// Gets the pickup sound played when the item is taken.
        /// </summary>
        public AudioClip PickupSound {
            get { return pickupSound; }
        }

        /// <summary>
        /// Gets the pickup sound played when the item is dropped
        public AudioClip DropSound {
            get { return dropSound; }
        }
    }
}