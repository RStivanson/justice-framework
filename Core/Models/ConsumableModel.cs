using System;
using System.Collections.Generic;

namespace JusticeFramework.Core.Models {
	/// <inheritdoc />
	/// <summary>
	/// Data class for consumables such as potions, food, etc.
	/// </summary>
	[Serializable]
	public class ConsumableModel : ItemModel {
        /// <summary>
        /// The status effects that will be applied to the model
        /// </summary>
        public List<StatusEffectModel> statusEffects;
	}
}