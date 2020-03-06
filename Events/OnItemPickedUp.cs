using JusticeFramework.Interfaces;

namespace JusticeFramework.Events {
	public delegate void OnItemPickedUp(IItem item, IContainer picker, int quantity);
}