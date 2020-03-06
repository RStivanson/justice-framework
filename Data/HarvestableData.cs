using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Model class that defines harvest information needed when harvest a flower object
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Harvestable Data")]
    public class HarvestableData : ScriptableDataObject, IDisplayable {
        /// <summary>
        /// Defines the gain from harvesting this node
        /// </summary>
        public struct HarvestResult {
            /// <summary>
            /// The item to be awarded upon harvesting
            /// </summary>
            [SerializeField]
            public ItemData itemData;

            /// <summary>
            /// The quantity of item given from a harvest
            /// </summary>
            [SerializeField]
            public int quantity;
        }

        /// <summary>
        /// The name to be displayed in game
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
		/// The item to be awarded upon harvesting
		/// </summary>
		[SerializeField]
        private HarvestResult harvestResult;

        /// <summary>
        /// The type of respawning mechanism to use
        /// </summary>
        [SerializeField]
        private EHarvestRespawnType respawnType;

        /// <summary>
        /// A count of seconds for this to regrow after being harvested
        /// </summary>
        [SerializeField]
        private int respawnTimeInSeconds;

        /// <summary>
        /// Sound clip to be played upon harvesting
        /// </summary>
        [SerializeField]
        private AudioClip harvestSound;

        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        public string DisplayName {
            get { return displayName; }
        }

        /// <summary>
        /// Gets the result from harvesting this node.
        /// </summary>
        public HarvestResult HarvestOutput {
            get { return harvestResult; }
        }

        /// <summary>
        /// Gets the type of respawning mechanism to use.
        /// </summary>
        public EHarvestRespawnType RespawnType {
            get { return respawnType; }
        }

        /// <summary>
        /// Gets the number of seconds needed this node to regrow.
        /// </summary>
        public int RespawnTimeInSeconds {
            get { return respawnTimeInSeconds; }
        }

        /// <summary>
        /// Gets the sound to be played when this object is harvested.
        /// </summary>
        public AudioClip HarvestSound {
            get { return harvestSound; }
        }
    }
}