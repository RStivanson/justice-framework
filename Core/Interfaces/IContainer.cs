using JusticeFramework.Core.Collections;

namespace JusticeFramework.Core.Interfaces {
	/// <summary>
	/// Interface that defines attributes needed for containers
	/// </summary>
	public interface IContainer {
		/// <summary>
		/// The list of items in this container
		/// </summary>
		Inventory Inventory { get; }
		
		/// <summary>
		/// Inserts an item into the container
		/// </summary>
		/// <param name="id">The Id of the item to put into the container</param>
		/// <param name="quantity">The amount of items to put into the container</param>
		void GiveItem(string id, int quantity);
		
		/// <summary>
		/// Removes an item from the container
		/// </summary>
		/// <param name="id">The Id of the item to remove from the container</param>
		/// <param name="quantity">The amount of items to remove from the container</param>
		bool TakeItem(string id, int quantity);

        /// <summary>
        /// Gets the quantity of item held
        /// </summary>
        /// <param name="id">The Id of the item to look for</param>
        /// <returns>Returns the number of items held in the container</returns>
        int GetQuantity(string id);
    }
}