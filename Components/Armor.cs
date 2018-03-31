using System;
using JusticeFramework.Data.Models;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Interfaces;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.Components {
	[Serializable]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))]
	public class Armor : Item, IArmor {
		public event OnItemEquipped OnItemEquipped;
		public event OnItemUnequipped OnItemUnequipped;
		
#region Variables
		
		[SerializeField]
		private SkinnedMeshRenderer thisRenderer;
		
		[SerializeField]
		private Rigidbody thisRigidbody;
		
		[SerializeField]
		private BoxCollider thisCollider;
		
		[SerializeField]
		private Transform defaultRootBone;
		
		[SerializeField]
		private Transform[] defaultBones;
		
#endregion

#region Properties

		private ArmorModel ArmorModel {
			get { return model as ArmorModel; }
		}
		
		public EEquipSlot EquipSlot {
			get { return ArmorModel.equipSlot; }
		}
		
		public AudioClip EquipSound {
			get { return ArmorModel.equipSound; }
		}
		
		public int ArmorRating {
			get { return ArmorModel.armorRating; }
		}

#endregion

		protected override void OnIntialize() {
			if (thisRenderer == null) {
				Debug.LogError($"Skinned mesh renderer on '{name}' has not been defined!");
			} else {
				defaultRootBone = thisRenderer.rootBone;
				defaultBones = thisRenderer.bones;
			}

			thisRigidbody = GetComponent<Rigidbody>();
			thisCollider = GetComponent<BoxCollider>();
		}

		/// <summary>
		/// Attaches this armor from the skeleton of the given actor
		/// </summary>
		/// <param name="actor">The currently attached actor</param>
		public void Equip(IActor actor) {
			GameManager.Instance.Unregister(this);

			SkinnedMeshRenderer actorRenderer = actor.Transform.GetComponentInChildren<SkinnedMeshRenderer>(true);
			
			transform.SetParent(actor.Transform);

			transform.localPosition = Vector3.zero;

			thisRigidbody.isKinematic = true;
			thisCollider.enabled = false;

			thisRenderer.rootBone = actorRenderer.rootBone;
			thisRenderer.bones = actorRenderer.bones;
			
			if (EquipSlot == EEquipSlot.Head) {
				thisRenderer.enabled = false;
			}
		}

		/// <summary>
		/// Unattaches this armor from the skeleton of the given actor
		/// </summary>
		/// <param name="actor">The currently attached actor</param>
		public void Unequip(IActor actor) {
			GameManager.Instance.Register(this);

			transform.parent = null;

			thisRenderer.rootBone = defaultRootBone;
			thisRenderer.bones = defaultBones;

			thisRigidbody.isKinematic = false;
			thisCollider.enabled = true;
		}
	}
}