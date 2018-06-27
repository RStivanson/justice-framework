using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Collections {
    public delegate void OnInventoryItemAdded(Inventory inventory, string id, int quantity);
    public delegate void OnInventoryItemRemoved(Inventory inventory, string id, int quantity);

    /// <summary>
    /// A collection of game items
    /// </summary>
    [Serializable]
	public class Inventory {
        /// <summary>
        /// Event called when an item is added to the inventory
        /// </summary>
        public event OnInventoryItemAdded onItemAdded;
        
        /// <summary>
        /// Event called when an item is removed from the inventory
        /// </summary>
        public event OnInventoryItemRemoved onItemRemoved;

		/// <summary>
		/// The contents of the item list
		/// </summary>
		[SerializeField]
		private List<ItemListEntry> contents;

		/// <summary>
		/// Gets the contents of the item list
		/// </summary>
		public List<ItemListEntry> Contents {
			get { return contents; }
		}

		/// <summary>
		/// Gets the amount of items in the item list
		/// </summary>
		public int Count {
			get { return Contents.Count; }
		}

		/// <summary>
		/// Gets the item with the specified ID
		/// </summary>
		/// <param name="id">The ID to search for</param>
		public ItemListEntry this[string id] {
			get { return Contents.Find(x => x.id.Equals(id)); }
		}

		/// <summary>
		/// Gets the items at the specified index
		/// </summary>
		/// <param name="key">The index to get the item at</param>
		public ItemListEntry this[int key] {
			get { return Contents[key]; }
		}

		/// <summary>
		/// Initializes a new instance of the the item list
		/// </summary>
		public Inventory() : this(null) { }

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
		/// <param name="weight">The indiviual item weight</param>
		public void AddItem(string id, int amount, float weight) {
			// If the amount is 0 or negative, do nothing
			if (id == null || amount <= 0) {
				return;
			}

			// Try to get the item from the contents
			ItemListEntry entry = contents.Find(x => x.id.Equals(id));

			// If an item entry was found then merge the quantities
			if (entry != null) {
				entry.count += amount;
			} else {
				// Else add a new item
				contents.Add(new ItemListEntry(id, amount, weight));
			}

            onItemAdded?.Invoke(this, id, amount);
        }

		/// <summary>
		/// Removes a item from the item list
		/// </summary>
		/// <param name="id"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public int RemoveItem(string id, int amount) {
			int removed = 0;

			// For each item starting at the end of the list (To avoid dealing with incorrect
			// indexing when an item is removed) while we have not run out of items and the
			// amount of items to remove is still greater than zero
			for (int i = contents.Count - 1; i >= 0 && amount > 0; --i) {
				// If this is not an item we are looking for, go to the next one
				if (!contents[i].id.Equals(id)) {
					continue;
				}

				// If the count of the item is greater than the amount we want to remove
				if (contents[i].count >= amount) {
					// Remove the amount from the item and update the return and remaining
					// amount fields
					contents[i].count -= amount;
					removed += amount;
					amount = 0;
				} else {
					// Remove the amount of the current entry from the remaining amount and add
					// it to the amount removed return value
					amount -= contents[i].count;
					removed += contents[i].count;
					
					// Ensure the items entry is at zero 
					contents[i].count = 0;
				}

				// The item has no more in it's stack, remove it from the list
				if (contents[i].count == 0) {
					contents.RemoveAt(i);
				}
			}

            onItemRemoved?.Invoke(this, id, removed);

			return removed;
		}

        /// <summary>
        /// Calculates the current weight stored in the inventory
        /// </summary>
        /// <returns>Returns the weight of all items in the inventory</returns>
        public float GetTotalWeight() {
            float weight = 0;

            // Get the weight from each item
            foreach (ItemListEntry item in contents) {
                weight += item.TotalStackWeight;
            }

            return weight;
        }

        /// <summary>
        /// Determines if the list contains the given item
        /// </summary>
        /// <param name="id">The Id of the item to search for</param>
        /// <returns>Returns true if the item list has the given Id, false otherwise</returns>
		public bool HasItem(string id) {
			return this[id] != null;
		}
		
		/// <summary>
		/// Deep copies the given item list
		/// </summary>
		/// <param name="itemList">The item list to copy data from</param>
		private void CopyList(Inventory itemList) {
			contents = new List<ItemListEntry>();

			if (itemList == null) {
				return;
			}
			
            // Copy each entry to this list
			foreach (ItemListEntry entry in itemList.Contents) {
				if (entry.count <= 0){
					continue;
				}

				contents.Add(new ItemListEntry(entry));
			}
		}
	}
}