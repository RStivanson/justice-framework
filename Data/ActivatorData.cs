using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    [CreateAssetMenu(menuName = "Justice Framework/Activator Data")]
    public class ActivatorData : ScriptableDataObject, IDisplayable {
        /// <summary>
        /// The name to be displayed in-game.
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
        /// Sound to be played when this entity is activated.
        /// </summary>
        [SerializeField]
        private AudioClip activationSound;

        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        public string DisplayName {
            get { return displayName; }
        }

        /// <summary>
        /// Gets the activation sound.
        /// </summary>
        public AudioClip ActivationSound {
            get { return activationSound; }
        }
    }
}
