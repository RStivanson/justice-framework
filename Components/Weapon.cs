using System;
using JusticeFramework.Data.Models;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.Components {
	[Serializable]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))]
	public class Weapon : Item, IWeapon {
		public event OnItemEquipped OnItemEquipped;
		public event OnItemUnequipped OnItemUnequipped;
		
#region Variables

		[SerializeField]
		private SkinnedMeshRenderer thisRenderer;

		[SerializeField]
		[HideInInspector]
		private Rigidbody thisRigidbody;

		[SerializeField]
		[HideInInspector]
		private BoxCollider thisCollider;

		[SerializeField]
		[HideInInspector]
		private Transform defaultRootBone;

		[SerializeField]
		[HideInInspector]
		private Transform[] defaultBones;

#endregion

#region Properties

		private WeaponModel WeaponModel {
			get { return model as WeaponModel; }
		}
		
		public EEquipSlot EquipSlot {
			get { return WeaponModel.equipSlot; }
		}

		public AudioClip EquipSound {
			get { return WeaponModel.equipSound; }
		}

		public EWeaponType WeaponType {
			get { return WeaponModel.weaponType; }
		}
		
		public int Damage {
			get { return WeaponModel.damage; }
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

		public void Equip(IActor actor) {
			SkinnedMeshRenderer actorRenderer = actor.Transform.GetComponentInChildren<SkinnedMeshRenderer>(true);
			
			transform.SetParent(actor.Transform);

			transform.localPosition = Vector3.zero;

			thisRigidbody.isKinematic = true;
			thisCollider.enabled = false;
			
			thisRenderer.rootBone = actorRenderer.rootBone;
			thisRenderer.bones = actorRenderer.bones;
		}

		public void Unequip(IActor actor) {
			transform.parent = null;

			thisRenderer.rootBone = defaultRootBone;
			thisRenderer.bones = defaultBones;

			thisRigidbody.isKinematic = false;
			thisCollider.enabled = true;
		}
	}
}