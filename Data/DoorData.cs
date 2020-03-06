using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Data class for all doors.
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Door Data")]
    public class DoorData : ScriptableDataObject, IDisplayable {
        /// <summary>
        /// The name to be displayed in-game.
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
        /// The type of door behaviour.
        /// </summary>
        [SerializeField]
        private EDoorType doorType;

        /// <summary>
        /// Sound clip to be played upon activation.
        /// </summary>
        [SerializeField]
        private AudioClip activationSound = null;

        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        public string DisplayName {
            get { return displayName; }
        }

        /// <summary>
        /// Gets the type of door behaviour.
        /// </summary>
        public EDoorType DoorType {
            get { return doorType; }
        }

        /// <summary>
        /// Gets the activation sound.
        /// </summary>
        public AudioClip ActivationSound {
            get { return activationSound; }
        }
    }
}