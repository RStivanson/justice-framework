using JusticeFramework.Data;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	public class EquippableModel : ItemModel {
		/// <summary>
		/// The slot where the armor should be equipped
		/// </summary>
		public EEquipSlot equipSlot = EEquipSlot.Head;
		
		/// <summary>
		/// Sound clip to be played upon equip
		/// </summary>
		public AudioClip equipSound = null;
	}
}