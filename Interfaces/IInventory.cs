using JusticeFramework.Core.Collections;
using JusticeFramework.Core.Interfaces;

namespace JusticeFramework.Interfaces {
	public delegate void OnItemAdded(IInventory reciever, string id, int amount);
	public delegate void OnItemRemoved(IInventory remover, string id, int amount);

	public interface IInventory {
		event OnItemAdded onItemAdded;
		event OnItemRemoved onItemRemoved;

		Inventory Inventory { get; }

		void GiveItem(string id, int amount);
		bool TakeItem(string id, int amount);

		void ActivateItem(string id);
	}
}