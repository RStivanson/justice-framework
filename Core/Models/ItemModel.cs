using System;
using UnityEngine;

namespace JusticeFramework.Core.Models {
	/// <inheritdoc />
	/// <summary>
	/// Base model class for all items.
	/// </summary>
	[Serializable]
	public class ItemModel : WorldObjectModel {
		/// <summary>
		/// The maximum amount of items that can be contained in a stack
		/// </summary>
		public const int MAX_STACK_AMOUNT = 999;

		/// <summary>
		/// The weight of the item. Affects encumberance.
		/// </summary>
		public float weight = 0;

		/// <summary>
		/// The value of the item to merchants
		/// </summary>
		public int value = 0;

		/// <summary>
		/// Determines if the item will stack in an inventory
		/// </summary>
		public bool isStackable = true;

		/// <summary>
		/// Sound clip to be played upon pickup
		/// </summary>
		public AudioClip pickupSound = null;

		/// <summary>
		/// Sound clip to be played upon removing the item from an inventory
		/// </summary>
		public AudioClip dropSound = null;
	}
}