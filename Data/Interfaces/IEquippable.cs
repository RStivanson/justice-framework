using JusticeFramework.Data.Events;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	public interface IEquippable : IItem {
		event OnItemEquipped OnItemEquipped;
		event OnItemUnequipped OnItemUnequipped;
		
		EEquipSlot EquipSlot { get; }
		AudioClip EquipSound { get; }

		void Equip(IActor actor);
		void Unequip(IActor actor);
	}
}