using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Model class for all containers such as chests or lockers.
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Chest Data")]
    public class ChestData : ScriptableDataObject, IDisplayable {
        /// <summary>
        /// The name to be displayed in-game.
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
        /// Sound clip to be played when opened.
        /// </summary>
        [SerializeField]
        private AudioClip openSound = null;

        /// <summary>
        /// A list of items to be given to this container when spawned for the first time.
        /// </summary>
        [SerializeField]
        private ItemStackData[] defaultInventory;

        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        public string DisplayName {
            get { return displayName; }
        }

        /// <summary>
        /// Gets the container's open sound.
        /// </summary>
        public AudioClip OpenSound {
            get { return openSound; }
        }

        /// <summary>
        /// Gets the default items to be given to this actor
        /// </summary>
        public ItemStackData[] DefaultInventory {
            get { return defaultInventory; }
        }
    }
}