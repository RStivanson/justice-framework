using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Data class for consumables such as potions, food, etc.
	/// </summary>
	[Serializable]
	public class ConsumableModel : ItemModel {
		/// <summary>
		/// How this consumable affects the actors health
		/// </summary>
		public int healthModifier = 0;
	}
}