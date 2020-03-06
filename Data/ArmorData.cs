using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Model class for all armor pieces.
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Armor Data")]
    public class ArmorData : ItemData {
        /// <summary>
        /// The object that should be spawned when this item is equipped.
        /// </summary>
        [SerializeField]
        private GameObject equipmentPrefab;

        /// <summary>
        /// The slot where the armor should be equipped.
        /// </summary>
        [SerializeField]
        private EEquipSlot equipSlot = EEquipSlot.Head;

        /// <summary>
        /// Sound clip to be played upon equip.
        /// </summary>
        [SerializeField]
        private AudioClip equipSound = null;

        /// <summary>
        /// The incoming damage reduction modifier.
        /// </summary>
        [SerializeField]
        private int armorRating = 0;

        /// <summary>
        /// Gets the prefab used when equipping this armor.
        /// </summary>
        public GameObject EquipPrefab {
            get { return equipmentPrefab; }
        }

        /// <summary>
        /// Gets the slot this armor is equipped to.
        /// </summary>
        public EEquipSlot EquipSlot {
            get { return equipSlot; }
        }

        /// <summary>
        /// Gets the sound played when this armor is equipped.
        /// </summary>
        public AudioClip EquipSound {
            get { return equipSound; }
        }

        /// <summary>
        /// Gets the armor rating valie. Plays into damage reduction.
        /// </summary>
        public int ArmorRating {
            get { return armorRating; }
        }
    }
}