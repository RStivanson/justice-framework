using System;
using JusticeFramework.Core.Models;
using JusticeFramework.Core;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
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
		private Collider thisCollider;
		
		[SerializeField]
		private Transform defaultRootBone;
		
		[SerializeField]
		private Transform[] defaultBones;

        private Actor owner;

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

        public Renderer Renderer {
            get { return thisRenderer; }
        }

        public Rigidbody Rigidbody {
            get { return thisRigidbody; }
        }

        public Collider Collider {
            get { return thisCollider; }
        }

        #endregion

        protected override void OnIntialized() {
			if (thisRenderer == null) {
				Debug.LogError($"Skinned mesh renderer on '{name}' has not been defined!");
			} else {
				defaultRootBone = thisRenderer.rootBone;
				defaultBones = thisRenderer.bones;
			}

			thisRigidbody = GetComponent<Rigidbody>();
			thisCollider = GetComponent<BoxCollider>();
		}

        public void SetOwner(WorldObject actor) {
            owner = actor as Actor;
        }

        /// <summary>
        /// Sets the bones of the object to the given renderer's bones
        /// </summary>
        /// <param name="renderer">The renderer to set the bones to</param>
        public void SetBones(SkinnedMeshRenderer renderer) {
			thisRenderer.rootBone = renderer.rootBone;
			thisRenderer.bones = renderer.bones;
		}

        /// <summary>
        /// Resets the objects bones to its original bones
        /// </summary>
        public void ClearBones() {
			thisRenderer.rootBone = defaultRootBone;
			thisRenderer.bones = defaultBones;
		}
	}
}