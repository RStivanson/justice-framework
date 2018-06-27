using System;
using UnityEngine;

namespace JusticeFramework.Core.Models {
    /// <summary>
    /// Base data class for all buffs an debuffs
    /// </summary>
    [Serializable]
    public class StatusEffectModel : ScriptableObject {
        /// <summary>
        /// The type of buff to be applied
        /// </summary>
        public EBuffType buffType;

        /// <summary>
        /// Flag indicating if this status effect can be stacked
        /// </summary>
        public bool isStackable;

        /// <summary>
        /// Flag indicating if the status effect is permanent
        /// </summary>
        public bool isPersistant;

        /// <summary>
        /// The length of time this status effect will last for
        /// </summary>
        public float duration;

        /// <summary>
        /// Flag indicating if the status effect should disolve after a single tick
        /// </summary>
        public bool isSingleTick;

        /// <summary>
        /// Length of time between each buff tick
        /// </summary>
        public float tickInterval;

        /// <summary>
        /// The amount of increase/decrease this buff may have
        /// </summary>
        public float modifier;
    }
}
