using JusticeFramework.Data.Events;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for equippable items
	/// </summary>
	public interface IEquippable : IItem {
		/// <summary>
		/// Event called when the item is equipped
		/// </summary>
		event OnItemEquipped OnItemEquipped;
		
		/// <summary>
		/// Event called when the item is unequipped
		/// </summary>
		event OnItemUnequipped OnItemUnequipped;
		
		/// <summary>
		/// The slot that this item should be equipped to
		/// </summary>
		EEquipSlot EquipSlot { get; }
		
		/// <summary>
		/// The sounds played when the item is equipped
		/// </summary>
		AudioClip EquipSound { get; }

		/// <summary>
		/// Equips the item to the given actor
		/// </summary>
		/// <param name="actor">The actor to equip the item to</param>
		void Equip(IActor actor);
		
		/// <summary>
		/// Unequips the item from the given actor
		/// </summary>
		/// <param name="actor">The actor to unequip the item from</param>
		void Unequip(IActor actor);
	}
}