using JusticeFramework.Collections;

namespace JusticeFramework.Interfaces {
	/// <summary>
	/// Interface that defines attributes needed for containers
	/// </summary>
	public interface IContainer {
		/// <summary>
		/// The list of items in this container
		/// </summary>
		Inventory Inventory { get; }
    }
}