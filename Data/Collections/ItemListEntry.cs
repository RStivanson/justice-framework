using System;

namespace JusticeFramework.Data.Collections {
	/// <summary>
	/// Data class used to store a item in an item list
	/// </summary>
	[Serializable]
	public class ItemListEntry {
		public string id;
		public int count;
		public float weightPer;

		/// <summary>
		/// Gets the total weight of the items in this stack
		/// </summary>
		public float TotalStackWeight {
			get { return count * weightPer; }
		}
		
		/// <summary>
		/// Initializes a new instance of an item entry
		/// </summary>
		/// <param name="id">The ID belonging to the item</param>
		/// <param name="count">The quantity of the item</param>
		/// <param name="weightPer">The individual weight for each item</param>
		public ItemListEntry(string id, int count, float weightPer) {
			this.id = id;
			this.count = count;
			this.weightPer = weightPer;
		}

		/// <summary>
		/// Intializes a new instance of an item entry with data copied from the provided
		/// item entry
		/// </summary>
		/// <param name="entry">The item entry to copy data from</param>
		public ItemListEntry(ItemListEntry entry) {
			id = entry.id;
			count = entry.count;
			weightPer = entry.weightPer;
		}
	}
}