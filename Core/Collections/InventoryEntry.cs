using System;
using UnityEngine;

namespace JusticeFramework.Core.Collections {
    /// <summary>
    /// Stores information regarding an item entry in an inventory
    /// </summary>
    [Serializable]
	public class InventoryEntry {
        /// <summary>
        /// THe item identifier
        /// </summary>
        [SerializeField]
		private string id;

        /// <summary>
        /// The stack information for the item
        /// </summary>
        [SerializeField]
		private Stack stack;

        /// <summary>
        /// Gets the identifier of this item
        /// </summary>
        public string Id {
            get { return id; }
        }

        /// <summary>
        /// Gets the stack information regarding this entry
        /// </summary>
        public Stack Stack {
            get { return stack; }
        }

        /// <summary>
        /// Gets a flag indicating if the entries stack is empty
        /// </summary>
        public bool IsEmpty {
            get { return stack.Quantity <= 0; }
        }

		/// <summary>
		/// Initializes a new instance of an item entry
		/// </summary>
		/// <param name="id">The ID belonging to the item</param>
		/// <param name="quantity">The quantity of the item</param>
		/// <param name="weightPer">The individual weight for each item</param>
		public InventoryEntry(string id, int quantity) {
			this.id = id;
			stack = new Stack(quantity);
		}

		/// <summary>
		/// Intializes a new instance of an item entry with data copied from the provided
		/// item entry
		/// </summary>
		/// <param name="entry">The item entry to copy data from</param>
		public InventoryEntry(InventoryEntry entry) {
			id = entry.id;
			stack = new Stack(entry.stack);
		}
	}
}