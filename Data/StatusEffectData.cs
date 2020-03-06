using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Setting object that contains data for overall game settings such as resolution and volume
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Status Effect Data", order = 104)]
    public class StatusEffectData : ScriptableDataObject {
        /// <summary>
        /// The type of buff to be applied.
        /// </summary>
        [SerializeField]
        private EBuffType buffType;

        /// <summary>
        /// Flag indicating if this status effect can be stacked.
        /// </summary>
        [SerializeField]
        private bool isStackable;

        /// <summary>
        /// Flag indicating if the status effect is permanent.
        /// </summary>
        [SerializeField]
        private bool isPersistant;

        /// <summary>
        /// The length of time this status effect will last for. Single tick = 0
        /// </summary>
        [SerializeField]
        private float durationInSeconds;

        /// <summary>
        /// Length of time between each effect tick.
        /// </summary>
        [SerializeField]
        private float tickIntervalInSeconds;

        /// <summary>
        /// The amount of increase/decrease this effect may have.
        /// </summary>
        [SerializeField]
        private float modifier;

        /// <summary>
        /// Gets the type of status effect to be applied.
        /// </summary>
        public EBuffType BuffType {
            get { return buffType; }
        }

        /// <summary>
        /// Gets if this effect can be stacked.
        /// </summary>
        public bool IsStackable {
            get { return isStackable; }
        }

        /// <summary>
        /// Gets if this effect is permanent.
        /// </summary>
        public bool IsPersistent {
            get { return isPersistant; }
        }

        /// <summary>
        /// Gets the duration of this status effect. Single tick = 0
        /// </summary>
        public float DurationInSeconds {
            get { return durationInSeconds; }
        }

        /// <summary>
        /// Gets the amount of seconds between each effect tick.
        /// </summary>
        public float TickIntervalInSeconds {
            get { return tickIntervalInSeconds; }
        }

        /// <summary>
        /// Gets the modification amount used by this effect.
        /// </summary>
        public float Modifier {
            get { return modifier; }
        }
    }
}
