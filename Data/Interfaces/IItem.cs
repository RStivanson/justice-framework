using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for items
	/// </summary>
	public interface IItem : IWorldObject {
		/// <summary>
		/// The weight of the item
		/// </summary>
		float Weight { get; }
		
		/// <summary>
		/// The ingame value of the item
		/// </summary>
		int Value { get; }
		
		/// <summary>
		/// Flag indicating if this item is stackable
		/// </summary>
		bool IsStackable { get; }
		
		/// <summary>
		/// The maximum amount of this item that can be contained in a stack
		/// </summary>
		int MaxStackAmount { get; }
		
		/// <summary>
		/// The sound played when this item is picked up
		/// </summary>
		AudioClip PickupSound { get; }
		
		/// <summary>
		/// The sound played when this item is dropped
		/// </summary>
		AudioClip DropSound { get; }
	}
}