using JusticeFramework.Core.Interfaces;

namespace JusticeFramework.Core.Events {
	public delegate void OnItemDropped(IItem item, IContainer dropped, int quantity);
}