using JusticeFramework.Data.Collections;

namespace JusticeFramework.Data.Interfaces {
	public interface IContainer {
		ItemList Inventory { get; }
		
		void GiveItem(string id, int quantity);
		void TakeItem(string id, int quantity);
	}
}