using JusticeFramework.Interfaces;

namespace JusticeFramework.Events {
	public delegate void OnItemDropped(IItem item, IContainer dropped, int quantity);
}