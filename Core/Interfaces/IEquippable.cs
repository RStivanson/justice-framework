using JusticeFramework.Components;
using JusticeFramework.Core.Events;
using UnityEngine;

namespace JusticeFramework.Core.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for equippable items
	/// </summary>
	public interface IEquippable : IItem {
		/// <summary>
		/// Event called when the item is equipped
		/// </summary>
		event OnItemEquipped onItemEquipped;
		
		/// <summary>
		/// Event called when the item is unequipped
		/// </summary>
		event OnItemUnequipped onItemUnequipped;
		
		/// <summary>
		/// The slot that this item should be equipped to
		/// </summary>
		EEquipSlot EquipSlot { get; }
		
		/// <summary>
		/// The sounds played when the item is equipped
		/// </summary>
		AudioClip EquipSound { get; }

        /// <summary>
        /// The renderer attached to this object
        /// </summary>
        Renderer Renderer { get; }

        /// <summary>
        /// The rigidbody attached to this object
        /// </summary>
        Rigidbody Rigidbody { get; }

        /// <summary>
        /// The collider attached to this object
        /// </summary>
        Collider Collider { get; }

        void SetOwner(WorldObject worldObject);
    }
}