using JusticeFramework.Data.Interfaces;

namespace JusticeFramework.Data.Events {
	public delegate void OnItemUnequipped(IEquippable unequipped, IActor unequippedFrom);
}