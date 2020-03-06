using JusticeFramework.Interfaces;

namespace JusticeFramework.Events {
	public delegate void OnItemUnequipped(IEquippable unequipped, IActor unequippedFrom);
}