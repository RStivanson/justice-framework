using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Data.Collections {
	/// <summary>
	/// A collection of game items
	/// </summary>
	[Serializable]
	public class ItemList {
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
		/// Gets or sets the total weight of all items in the item list
		/// </summary>
		public float TotalWeight {
			get {
				float weight = 0;

				foreach (ItemListEntry item in contents) {
					weight += item.TotalStackWeight;
				}

				return weight;
			}
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
		public ItemList() : this(null) { }

		/// <summary>
		/// Initializes a new instance of the item list with the same data as the passed
		/// item list
		/// </summary>
		/// <param name="itemList">The item list to copy</param>
		public ItemList(ItemList itemList) {
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

			return removed;
		}

		public bool HasItem(string id) {
			return this[id] != null;
		}
		
		/// <summary>
		/// Deep copies the given item list
		/// </summary>
		/// <param name="itemList">The item list to copy data from</param>
		private void CopyList(ItemList itemList) {
			contents = new List<ItemListEntry>();

			if (itemList == null) {
				return;
			}
			
			foreach (ItemListEntry entry in itemList.Contents) {
				if (entry.count <= 0) {
					continue;
				}

				contents.Add(new ItemListEntry(entry));
			}
		}
	}
}