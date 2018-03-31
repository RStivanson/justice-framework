using JusticeFramework.Data.Interfaces;

namespace JusticeFramework.Data.Events {
	public delegate void OnItemDropped(IItem item, IContainer dropped, int quantity);
}