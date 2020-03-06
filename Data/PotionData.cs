using System;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Data class for consumables such as potions, food, etc.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "Justice Framework/Potion Data")]
    public class PotionData : ItemData {
        /// <summary>
        /// The status effects that will be applied to the model
        /// </summary>
        [SerializeField]
        private StatusEffectData[] statusEffects;

        public StatusEffectData[] StatusEffects {
            get { return statusEffects; }
        }
    }
}