using JusticeFramework.Core.Interfaces;

namespace JusticeFramework.Core.Events {
	public delegate void OnItemEquipped(IEquippable equipped, IActor equippedTo);
}