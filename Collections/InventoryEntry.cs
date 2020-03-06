using JusticeFramework.Data;
using JusticeFramework.Managers;
using System;
using UnityEngine;

namespace JusticeFramework.Collections {
    /// <summary>
    /// Stores information regarding an item entry in an inventory
    /// </summary>
    [Serializable]
	public class InventoryEntry {
        /// <summary>
        /// The item data class associated with this inventory entry
        /// </summary>
        [SerializeField]
		private ItemData itemData;

        /// <summary>
        /// The current amount of items being held in this stack
        /// </summary>
        [SerializeField]
		private int quantity;

        /// <summary>
        /// Gets the identifier of this item
        /// </summary>
        public string Id {
            get { return itemData.Id; }
        }

        /// <summary>
        /// Gets the current stack capacity
        /// </summary>
        public int Quantity {
            get { return quantity; }
        }

        /// <summary>
        /// Gets the item data associated with this inventory entry.
        /// </summary>
        public ItemData ItemData {
            get {
                return itemData;
            }
        }

        /// <summary>
        /// Gets a flag indicating if the entries stack is empty
        /// </summary>
        public bool IsEmpty {
            get { return quantity <= 0; }
        }

		/// <summary>
		/// Initializes a new instance of an item entry
		/// </summary>
		/// <param name="id">The ID belonging to the item</param>
		/// <param name="quantity">The quantity of the item</param>
		/// <param name="weightPer">The individual weight for each item</param>
		public InventoryEntry(string id, int quantity) {
            itemData = GameManager.DataManager.GetAssetById<ItemData>(id);
            this.quantity = quantity;
		}

		/// <summary>
		/// Intializes a new instance of an item entry with data copied from the provided
		/// item entry
		/// </summary>
		/// <param name="entry">The item entry to copy data from</param>
		public InventoryEntry(InventoryEntry entry) {
            itemData = entry.itemData;
            quantity = entry.quantity;
        }

        /// <summary>
        /// Adds the given amount to the stack
        /// </summary>
        /// <param name="amount">The amount to add</param>
        public void Add(int amount) {
            // Add the amount to the stack
            quantity += amount;
        }

        /// <summary>
        /// Removes the amount of items from the given stack
        /// </summary>
        /// <param name="amount">The amount of items to remove from the stack</param>
        /// <returns>Returns the capacity remaining</returns>
        public int Remove(int amount) {
            int amountRemoved = 0;
            int previous = quantity;

            // Remove the quantity from the stack
            quantity -= amount;

            // If we are under zero
            if (quantity < 0) {
                // Calculate the actual amount removed
                amountRemoved = previous;

                // Reset the stack to 0
                quantity = 0;
            } else {
                // We removed everything
                amountRemoved = amount;
            }

            return amount - amountRemoved;
        }
    }
}