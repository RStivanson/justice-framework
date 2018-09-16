using JusticeFramework.Components;
using JusticeFramework.Core.Collections;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models;
using JusticeFramework.Core.UI;
using JusticeFramework.UI.Components;
using JusticeFramework.Utility.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    public delegate void OnContainerAction(ContainerView.ContainerItem item);

    [Serializable]
    public class ContainerView : Window {
        public class ContainerItem {
            public IContainer Owner { get; private set; }

            public ItemModel Model { get; private set; }

            public Stack Stack { get; private set; }

            public bool IsEquipped { get; private set; }

            public ContainerItem(IContainer owner, ItemModel model, Stack stack, bool isEquipped) {
                Owner = owner;
                Model = model;
                Stack = stack;
                IsEquipped = isEquipped;
            }
        }

        private enum EContainerTabs {
            Player,
            Target            
        }

        private const KeyCode TransferKeyCode = KeyCode.R;
        private const KeyCode ActivateKeyCode = KeyCode.E;

        [SerializeField]
        private Text selectedItemNameLabel;

        [SerializeField]
        private Text selectedItemDescrLabel;

        [SerializeField]
        private GameObject itemButtonPrefab;
        
        [SerializeField]
        private Transform playerItemContainer;
        
        [SerializeField]
        private Transform targetItemContainer;

        [SerializeField]
        private GameObject targetItemListPanel;
        
        private EContainerTabs currentTab = EContainerTabs.Player;
        private IContainer firstContainer;
        private IContainer secondContainer;
        private bool showEquippedItems;
        private ContainerItem selectedItem;

        #region Unity Events

        private void Update() {
            // If we do not have any item selected, return
            if (selectedItem == null) {
                return;
            }

            switch (currentTab) {
                case EContainerTabs.Player:
                    if (secondContainer != null && Input.GetKeyDown(TransferKeyCode)) {
                        TransferItem(selectedItem, secondContainer);
                        UpdateItemList(firstContainer, playerItemContainer);
                        UpdateItemList(secondContainer, targetItemContainer);
                    }

                    if (Input.GetKeyDown(ActivateKeyCode)) {
                        ActivateItem(selectedItem, secondContainer);
                        UpdateItemList(firstContainer, playerItemContainer);
                    }

                    break;
                case EContainerTabs.Target:
                    if (Input.GetKeyDown(TransferKeyCode)) {
                        TransferItem(selectedItem, firstContainer);
                        UpdateItemList(firstContainer, playerItemContainer);
                        UpdateItemList(secondContainer, targetItemContainer);
                    }

                    break;
            }
        }

        #endregion

        #region Event Callbacks

        protected virtual void OnItemSelected(ContainerItem item) {
            selectedItem = item;
            currentTab = ReferenceEquals(item.Owner, secondContainer) ? EContainerTabs.Target : EContainerTabs.Player;

            UpdateDescriptionFields(item);
        }

        #endregion

        #region UI Functions

        protected virtual void UpdateDescriptionFields(ContainerItem itemData) {
            selectedItemNameLabel.text = itemData.Model.displayName;

            if (itemData.Stack.Quantity > 1) {
                selectedItemNameLabel.text += $" (x{itemData.Stack.Quantity})";
            }

            if (itemData.IsEquipped) {
                selectedItemNameLabel.text += $" (E)";
            }

            // TODO: Overhaul: IHasStatusEffects?
            selectedItemDescrLabel.enabled = false;

            if (itemData.Model is ConsumableModel) {
                ConsumableModel consumable = (ConsumableModel)itemData.Model;
                selectedItemDescrLabel.text = "Effects:\n";

                foreach (StatusEffectModel statusEffectModel in consumable.statusEffects) {
                    selectedItemDescrLabel.text += $"\t\t{statusEffectModel.buffType.ToString()}\n";
                }

                selectedItemDescrLabel.enabled = true;
            }
        }

        #endregion

        #region Container Functions

        /// <summary>
        /// Transfers an item between the owning and target container
        /// </summary>
        /// <param name="owner">The item's owning container</param>
        /// <param name="targetContainer">The item's target container</param>
        /// <param name="model">The model data for the item</param>
        /// <param name="stack">The stack data for the item</param>
        /// <param name="isEquipped">Flag indicating if this item is currently equipped</param>
        protected virtual void TransferItem(ContainerItem itemData, IContainer targetContainer) {
            // If the item can't be transfered, do nothing
            if (!CanTransferItem(itemData, targetContainer)) {
                return;
            }

            if (targetContainer != null) {
                targetContainer.Inventory.Add(itemData.Model.id, itemData.Stack.Quantity);
                itemData.Owner.Inventory.Remove(itemData.Model.id, itemData.Stack.Quantity);
            } else {
                // Remove and spawn the item
                itemData.Owner.Inventory.Remove(itemData.Model.id, itemData.Stack.Quantity);
                IItem item = GameManager.SpawnAtPlayer(itemData.Model.id) as IItem;

                // If the object is an item and it exists
                if (item != null) {
                    // Update its stack amount
                    item.StackAmount = itemData.Stack.Quantity;
                }
            }

            OnItemTransfered(itemData, targetContainer);
        }

        /// <summary>
        /// Determines if the item is able to be transfered between containers
        /// </summary>
        /// <param name="owner">The item's owning container</param>
        /// <param name="targetContainer">The item's target container</param>
        /// <param name="model">The model data for the item</param>
        /// <param name="stack">The stack data for the item</param>
        /// <param name="isEquipped">Flag indicating if this item is currently equipped</param>
        /// <returns>Returns true if the item can be transfered, false toherwise</returns>
        protected virtual bool CanTransferItem(ContainerItem itemData, IContainer targetContainer) {
            return !itemData.IsEquipped;
        }

        /// <summary>
        /// Event method called when an item is transferred between containers
        /// </summary>
        /// <param name="owner">The item's owning container</param>
        /// <param name="targetContainer">The item's target container</param>
        /// <param name="model">The model data for the item</param>
        /// <param name="stack">The stack data for the item</param>
        /// <param name="isEquipped">Flag indicating if this item is currently equipped</param>
        protected virtual void OnItemTransfered(ContainerItem itemData, IContainer targetContainer) {
        }

        protected virtual void ActivateItem(ContainerItem itemData, IContainer targetContainer) {
            if (itemData.Owner.NotType<Actor>()) {
                return;
            }

            Actor actor = (Actor)itemData.Owner;

            if (itemData.Model is ConsumableModel) {
                ConsumableModel consumable = (ConsumableModel)itemData.Model;

                actor.Inventory.Remove(itemData.Model.id, 1);
                actor.Consume(consumable);
            } else if (itemData.Model is EquippableModel) {
                EquippableModel equippable = (EquippableModel)itemData.Model;

                if (itemData.IsEquipped) {
                    Actor.Unequip(actor, actor.Inventory, equippable.equipSlot);
                } else {
                    Actor.Equip(actor, actor.Inventory, equippable.id);
                }
            }

            OnItemActivated(itemData, targetContainer);
        }


        /// <summary>
        /// Determines if the item is able to be activated by the owner
        /// </summary>
        /// <param name="owner">The item's owning container</param>
        /// <param name="targetContainer">The item's target container</param>
        /// <param name="model">The model data for the item</param>
        /// <param name="stack">The stack data for the item</param>
        /// <param name="isEquipped">Flag indicating if this item is currently equipped</param>
        /// <returns>Returns true if the item can be transfered, false toherwise</returns>
        protected virtual bool CanActivateItem(ContainerItem itemData, IContainer targetContainer) {
            return targetContainer == null;
        }

        protected virtual void OnItemActivated(ContainerItem itemData, IContainer targetContainer) {
        }

        #endregion

        #region UI Functions

        private void UpdateItemList(IContainer container, Transform listContainer) {
            listContainer.DestroyAllChildren();

            for (int i = 0; i < container.Inventory.Count; ++i) {
                InventoryEntry entry = container.Inventory[i];
                ItemModel model = GameManager.AssetManager.GetById<ItemModel>(entry.Id);

                if (model == null) {
                    continue;
                }

                CreateListButton(listContainer, container, model, entry.Stack, false);
            }
        }

        private void CreateListButton(Transform listContainer, IContainer owner, ItemModel model, Stack stack, bool isEquipped) {
            if (stack.Quantity > 1 && !model.isStackable) {
                for (int i = 0; i < stack.Quantity; i++) {
                    AddButton(listContainer, owner, model, stack, isEquipped);
                }
            } else {
                AddButton(listContainer, owner, model, stack, isEquipped);
            }
        }

        private void AddButton(Transform container, IContainer owner, ItemModel model, Stack stack, bool isEquipped) {
            GameObject spawnedObject = Instantiate(itemButtonPrefab);

            spawnedObject.GetComponent<ContainerListButton>().SetItem(new ContainerItem(owner, model, stack, isEquipped), OnItemSelected);

            spawnedObject.transform.SetParent(container, false);
        }

        private void ClearSelectedItem() {
            selectedItem = null;

            selectedItemNameLabel.text = string.Empty;
            selectedItemDescrLabel.enabled = false;
        }

        #endregion

        public void View(IContainer first, IContainer second, bool showEquipped = false) {
            if (first == null) {
                return;
            }

            firstContainer = first;
            secondContainer = second;
            showEquippedItems = showEquipped;

            UpdateItemList(first, playerItemContainer);

            if (second != null) {
                targetItemListPanel?.SetActive(true);
                UpdateItemList(second, targetItemContainer);
            }
            
            ClearSelectedItem();
            Show();
        }
    }
}