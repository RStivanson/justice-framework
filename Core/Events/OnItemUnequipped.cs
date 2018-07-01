using JusticeFramework.Core.Interfaces;

namespace JusticeFramework.Core.Events {
	public delegate void OnItemUnequipped(IEquippable unequipped, IActor unequippedFrom);
}