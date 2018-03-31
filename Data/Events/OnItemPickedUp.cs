using JusticeFramework.Data.Interfaces;

namespace JusticeFramework.Data.Events {
	public delegate void OnItemPickedUp(IItem item, IContainer picker, int quantity);
}