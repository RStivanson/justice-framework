using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Collections {
    public delegate void OnInventoryItemAction(Inventory inventory, string id, int quantity);

    /// <summary>
    /// A collection of game items
    /// </summary>
    [Serializable]
	public class Inventory {
        /// <summary>
        /// Event called when an item is added to the inventory
        /// </summary>
        public event OnInventoryItemAction onItemAdded;
        
        /// <summary>
        /// Event called when an item is removed from the inventory
        /// </summary>
        public event OnInventoryItemAction onItemRemoved;

		/// <summary>
		/// The contents of the item list
		/// </summary>
		[SerializeField]
		private List<InventoryEntry> contents;

		/// <summary>
		/// Gets the contents of the item list
		/// </summary>
		public List<InventoryEntry> Contents {
			get { return contents; }
		}

		/// <summary>
		/// Gets the amount of items in the item list
		/// </summary>
		public int Count {
			get { return Contents.Count; }
		}

		/// <summary>
		/// Gets the item with the specified Id
		/// </summary>
		/// <param name="id">The ID to search for</param>
		public InventoryEntry this[string id] {
			get {
                int index = IndexOf(id);
                return index != -1 ? contents[index] : null;
            }
		}

		/// <summary>
		/// Gets the items at the specified index
		/// </summary>
		/// <param name="key">The index to get the item at</param>
		public InventoryEntry this[int key] {
			get { return Contents[key]; }
		}

		/// <summary>
		/// Initializes a new instance of the the item list
		/// </summary>
		public Inventory() {
            contents = new List<InventoryEntry>();
        }

		/// <summary>
		/// Initializes a new instance of the item list with the same data as the passed
		/// item list
		/// </summary>
		/// <param name="itemList">The item list to copy</param>
		public Inventory(Inventory itemList) {
			CopyList(itemList);
		}

		/// <summary>
		/// Adds the specified item values to the item list
		/// </summary>
		/// <param name="id">The ID of the item</param>
		/// <param name="amount">The quantity of the item</param>
		public void Add(string id, int amount) {
			// If the amount is 0 or negative, do nothing
			if (amount <= 0) {
				return;
			}

            // Get the index of the item already stored
            int itemIndex = IndexOf(id);

            // If the item exists
            if (itemIndex != -1) {
                // Add to its stack
                contents[itemIndex].Stack.Add(amount);
            } else {
                // Create a new entry
                contents.Add(new InventoryEntry(id, amount));
            }

            // Invoke item added event
            onItemAdded?.Invoke(this, id, amount);
        }

		/// <summary>
		/// Removes a item from the item list
		/// </summary>
		/// <param name="id">The item Id to remove from the list</param>
		/// <param name="amount">The amount of the item to remove</param>
		/// <returns>Returns the amount remaining from the original remove amount</returns>
		public int Remove(string id, int amount) {
			int remaining = amount;
            InventoryEntry entry;
            
			// For each item starting at the end of the list (To avoid dealing with incorrect
			// indexing when an item is removed) while we have not run out of items and the
			// amount of items to remove is still greater than zero
			for (int i = contents.Count - 1; i >= 0 && remaining > 0; --i) {
                entry = contents[i];

				// If this is not an item we are looking for, go to the next one
				if (!entry.Id.Equals(id)) {
					continue;
				}

                // Remove the from the items stack and update the remaining by the amount removed
                remaining = entry.Stack.Remove(remaining);
                
				// The item has no more in it's stack, remove it from the list
				if (entry.IsEmpty) {
					contents.RemoveAt(i);
				}
			}

            // Invoke item removed event
            onItemRemoved?.Invoke(this, id, amount - remaining);

			return remaining;
		}

        /// <summary>
        /// Determines if the list contains the given item and that item has a quantity greater than zero
        /// </summary>
        /// <param name="id">The Id of the item to look for</param>
        /// <returns>Returns true if the item list has the given Id with a non zero quantity, false otherwise</returns>
		public bool Contains(string id) {
            return Contains(id, 1);
        }

        /// <summary>
        /// Determines if the list contains the given item and that item has a quantity greater than the specified quantity
        /// </summary>
        /// <param name="id">The Id of the item to look for</param>
        /// <returns>Returns true if the item list has the given Id with at least the given quantity, false otherwise</returns>
		public bool Contains(string id, int minimumQuantity) {
            int index = IndexOf(id);
            return index != -1 && contents[index].Stack.Quantity >= minimumQuantity;
        }

        /// <summary>
        /// Gets the quantity of the item in the inventory
        /// </summary>
        /// <param name="id">The Id of the item to look for</param>
        /// <returns>Returns the quantity of the item in the inventory, or 0 if the item is not in the inventory</returns>
        public int GetQuantity(string id) {
            int itemIndex = IndexOf(id);
            return itemIndex != -1 ? contents[itemIndex].Stack.Quantity : 0;
        }

        /// <summary>
        /// Gets the index of an item in the inventory
        /// </summary>
        /// <param name="id">The Id of the item to look for</param>
        /// <returns>Returns the index of the item, -1 if it isn't found</returns>
        private int IndexOf(string id) {
            int index = -1;

            // For each item in the list
            for (int i = 0; i < contents.Count && index == -1; i++) {
                // If the Ids match, get the index
                if (contents[i].Id.Equals(id)) {
                    index = i;
                }
            }

            return index;
        }

		/// <summary>
		/// Deep copies the given inventories contents
		/// </summary>
		/// <param name="inventory">The inventory to copy data from</param>
		private void CopyList(Inventory inventory) {
			contents = new List<InventoryEntry>();
            
            // If the list we got is invalid, do nothing
			if (inventory == null) {
				return;
			}
			
            // Copy each entry to this list
			foreach (InventoryEntry entry in inventory.Contents) {
                // If the entry is empty, don't add it
				if (entry.IsEmpty){
					continue;
				}

                // Add a copy of the entry to our contents
				contents.Add(new InventoryEntry(entry));
			}
		}
	}
}