using JusticeFramework.Data.Interfaces;

namespace JusticeFramework.Data.Events {
	public delegate void OnItemEquipped(IEquippable equipped, IActor equippedTo);
}