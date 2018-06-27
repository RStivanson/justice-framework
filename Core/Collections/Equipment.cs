using JusticeFramework.Core.Interfaces;
using System;
using UnityEngine;

namespace JusticeFramework.Core.Collections {
    [Serializable]
    public class Equipment {
        [SerializeField]
        private IEquippable[] equipment;

        public Equipment() {
            equipment = new IEquippable[Enum.GetNames(typeof(EEquipSlot)).Length];
        }

        /// <summary>
        /// Attempts to equip the item
        /// </summary>
        /// <param name="item">The equippable item to attach to the actor</param>
        /// <returns>Return true if the item is equipped, false otherwise</returns>
		public bool Equip(IEquippable item, Transform attachBone, SkinnedMeshRenderer meshRenderer) {
            // If the item is empty, do nothing
            if (item == null) {
                return false;
            }

            // Make sure the object is not reacting to physics
            item.Rigidbody.isKinematic = true;
            item.Collider.enabled = false;

            if (item is IArmor) { // If this is armor
                IArmor armor = item as IArmor;

                // Set the parent and override the bones with the actors bones
                armor.Transform.SetParent(attachBone);
                armor.SetBones(meshRenderer);
            } else if (item is IWeapon) { // If this is a weapon
                IWeapon weapon = item as IWeapon;

                // Set the parent and reset the position and rotation to the actors hand
                weapon.Transform.SetParent(attachBone);
                weapon.Transform.localPosition = Vector3.zero;
                weapon.Transform.rotation = attachBone.rotation;

                // Provide an offhand IK target
                if (weapon.OffhandIkTarget != null) {
                    // TODO : Do some offhand IK stuff here
                }
            }

            // Unequip other pieces and store this item
            Unequip(item);
            equipment[(int)item.EquipSlot] = item;

            return true;
        }

        /// <summary>
        /// Provides unequip functionality for a specific type of equippable
        /// </summary>
        /// <param name="equippable">The equippable to attempt to unequip</param>
        protected bool Unequip(IEquippable equippable) {
            IWeapon weapon = equippable as IWeapon;
            bool unequipped = false;

            // If this is a two handed weapon
            if (weapon != null && weapon.WeaponType == EWeaponType.TwoHanded) {
                // Make sure the mainhand is unequipped
                if (equipment[(int)EEquipSlot.Mainhand] != null) {
                    unequipped = Unequip(EEquipSlot.Mainhand);
                }

                // Also make sure the offhand is unequipped
                if (equipment[(int)EEquipSlot.Offhand] != null) {
                    unequipped |= Unequip(EEquipSlot.Offhand);
                }
            } else {
                // If this is any other type of item, just make sure its slot is unequipped
                if (equipment[(int)equippable.EquipSlot] != null) {
                    unequipped = Unequip(equippable.EquipSlot);
                }
            }

            return unequipped;
        }

        /// <summary>
        /// Unequips an item from the given equipment spot and either drops it or adds it to the actor's inventory
        /// </summary>
        /// <param name="equipSlot">The equipment slot to unequip from</param>
        /// <param name="inventory">The inventory to add the item to</param>
        /// <returns>Return true if an item was unequipped, false otherwise</returns>
        public bool Unequip(EEquipSlot equipSlot, Inventory inventory = null) {
            IEquippable item = equipment[(int)equipSlot];

            // If the item is null, do nothing
            if (item == null) {
                return false;
            }

            if (item is IArmor) {
                IArmor armor = item as IArmor;

                // Reset the bones and make sure the armor is rendering
                armor.ClearBones();
                armor.Renderer.enabled = true;
            } else if (item is IWeapon) {
                IWeapon weapon = item as IWeapon;

                // Provide IK movement for the actor's offhand if needed
                if (weapon.OffhandIkTarget != null) {
                    // TODO : Do some offhand IK stuff here
                }
            }

            // If the item should be dropped
            if (inventory == null) {
                // Reset the objects components
                item.Rigidbody.isKinematic = false;
                item.Collider.enabled = true;

                // Unparent the object
                item.Transform.SetParent(null);
            } else {
                // Add the item to the actor's inventory and get rid of the GameObject
                inventory.AddItem(item.Id, 1, item.Weight);
                GameObject.Destroy(item.Transform.gameObject);
            }

            return true;
        }

        public IEquippable GetEquipment(EEquipSlot slot) {
            return equipment[(int)slot];
        }
    }
}
