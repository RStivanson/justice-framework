using System;
using UnityEngine;

namespace JusticeFramework.Core.Models {
	/// <inheritdoc />
	/// <summary>
	/// Model class the defines shared properties for objects that are equippable to an actor
	/// </summary>
	[Serializable]
	public class EquippableModel : ItemModel {
        /// <summary>
        /// The object that should be spawned when this item is equipped
        /// </summary>
        public GameObject equipmentPrefab;
        
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