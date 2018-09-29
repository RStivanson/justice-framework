using JusticeFramework.Core.Interfaces;
using System;
using UnityEngine;

namespace JusticeFramework.Core.Collections {
    /// <summary>
    /// A collection of equipped game items
    /// </summary>
    [Serializable]
    public class Equipment {
        /// <summary>
        /// A list of current equipped items indexed by <ref>EEquipSlot</ref>
        /// </summary>
        [SerializeField]
        private EquippedItem[] equipment;

        public EquippedItem[] Items {
            get { return equipment; }
        }

        /// <summary>
        /// Gets the equipped item using the equip slot
        /// </summary>
        /// <param name="slot">The slot to get the item from</param>
        /// <returns>Returns the item equipped in the slot, null if nothing is equipped there</returns>
        public IEquippable this[EEquipSlot slot] {
            get { return equipment[(int)slot]?.EquippedObject ?? null; }
        }

        /// <summary>
        /// Constructs a new equipment object
        /// </summary>
        public Equipment() {
            equipment = new EquippedItem[Enum.GetNames(typeof(EEquipSlot)).Length];
        }

        /// <summary>
        /// Gets the equipped item in the given slot and converts it
        /// </summary>
        /// <typeparam name="T">The type to convert to</typeparam>
        /// <param name="slot">The slot to get the item from</param>
        /// <returns>Returns the equipped item from the slot as the type, or null if it doesn't exit</returns>
        public T Get<T>(EEquipSlot slot) where T : class, IEquippable {
            return this[slot] as T;
        }

        /// <summary>
        /// Equips the given item
        /// </summary>
        /// <param name="item">The item to equip</param>
        /// <param name="attachTo">The transform to parent the item to</param>
        /// <param name="meshRenderer">The skinned mesh renderer that has the parents bones</param>
        /// <returns>Return true if the item is equipped, false otherwise</returns>
        public IEquippable Equip(IEquippable item, Transform attachTo, PerspectiveAnimator animator = null, SkinnedMeshRenderer meshRenderer = null) {
            // If the item is empty, do nothing
            if (item == null) {
                return null;
            }

            // Make sure the object is not reacting to physics
            item.Rigidbody.isKinematic = true;
            item.Collider.enabled = false;
            
            // Set the parent and reset the position and rotation to the attachment point
            item.Transform.SetParent(attachTo);
            item.Transform.localPosition = Vector3.zero;
            item.Transform.rotation = attachTo.rotation;

            // If this item needs to be rigged
            if (meshRenderer != null && item is IRigged) {
                IRigged riggedItem = (IRigged)item;

                // Update the items bones
                riggedItem.SetBones(meshRenderer);
            }

            // If this is a weapon
            if (item is IWeapon) {
                IWeapon weapon = item as IWeapon;

                // Provide an offhand IK target if it has one
                if (weapon.OffhandIkTarget != null) {
                    // TODO : Do some offhand IK stuff here
                }

                animator?.SetOverrideController(weapon.FPOverrideController, weapon.TPOverrideController);
            }

            // Unequip any item aready in this slot
            IEquippable unequipped = Unequip(item.EquipSlot);

            // Put the new item in the slot
            equipment[(int)item.EquipSlot] = new EquippedItem(item, animator);

            // return the unequipped item
            return unequipped;
        }

        /// <summary>
        /// Unequips a item
        /// </summary>
        /// <param name="slot">The slot to unequip from</param>
        /// <returns>Returns the item unequipped, or null if nothing was unequipped</returns>
        public IEquippable Unequip(EEquipSlot slot) {
            int index = (int)slot;
            EquippedItem equipItem = equipment[index];
            equipment[index] = new EquippedItem(null, null);

            // If the item isn't null
            if (equipItem != null && equipItem.EquippedObject != null) {
                // If this is a rigged item
                if (equipItem.EquippedObject is IRigged) {
                    IRigged riggedItem = equipItem.EquippedObject as IRigged;

                    // Clear the overriden rig bones
                    riggedItem.ClearBones();
                }

                if (equipItem.EquippedObject is IWeapon) {
                    equipItem.Animator?.ResetOverrideControllers();
                }

                // Reset physics and rendering data
                equipItem.EquippedObject.Rigidbody.isKinematic = false;
                equipItem.EquippedObject.Collider.enabled = true;
                equipItem.EquippedObject.Renderer.enabled = true;

                // Clear the items parent
                equipItem.EquippedObject.Transform.SetParent(null);
            }

            return equipItem?.EquippedObject ?? null;
        }
    }
}
