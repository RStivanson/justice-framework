using JusticeFramework.Core.Interfaces;

namespace JusticeFramework.Core.Events {
	public delegate void OnItemPickedUp(IItem item, IContainer picker, int quantity);
}