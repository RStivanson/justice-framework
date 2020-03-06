using JusticeFramework.Collections;
using JusticeFramework.Components;
using JusticeFramework.Core.Extensions;
using JusticeFramework.Core.UI;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using JusticeFramework.Managers;
using JusticeFramework.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    public delegate void OnContainerAction(ContainerView.ContainerItem item);

    [Serializable]
    public class ContainerView : Window {
        public class ContainerItem {
            public IContainer Owner { get; private set; }

            public ItemData Model { get; private set; }

            public int Quantity { get; private set; }

            public IEquippable EquippedItem { get; private set; }

            public bool IsEquipped { get; private set; }

            public ContainerItem(IContainer owner, ItemData model, int quantity, bool isEquipped) {
                Owner = owner;
                Model = model;
                Quantity = quantity;
                EquippedItem = null;
                IsEquipped = isEquipped;
            }

            public ContainerItem(IContainer owner, IEquippable equippedItem, bool isEquipped) {
                Owner = owner;
                Model = null;
                Quantity = 0;
                EquippedItem = equippedItem;
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

        [SerializeField]
        private GameObject categoryHeaderPrefab;

        private EContainerTabs currentTab = EContainerTabs.Player;
        private IContainer sourceContainer;
        private IContainer targetContainer;
        private EContainerViewMask sourceItemMask;
        private EContainerViewMask targetItemMask;
        private ContainerItem selectedItem;

        #region Unity Events

        private void Update() {
            // If we do not have any item selected, return
            if (selectedItem == null) {
                return;
            }

            switch (currentTab) {
                case EContainerTabs.Player:
                    if (targetContainer != null && Input.GetKeyDown(TransferKeyCode)) {
                        TransferItem(selectedItem, targetContainer);
                        UpdateItemList(sourceContainer, sourceItemMask, playerItemContainer);
                        UpdateItemList(targetContainer, targetItemMask, targetItemContainer);
                    }

                    if (Input.GetKeyDown(ActivateKeyCode)) {
                        ActivateItem(selectedItem, targetContainer);
                        UpdateItemList(sourceContainer, sourceItemMask, playerItemContainer);
                    }

                    break;
                case EContainerTabs.Target:
                    if (Input.GetKeyDown(TransferKeyCode)) {
                        TransferItem(selectedItem, sourceContainer);
                        UpdateItemList(sourceContainer, sourceItemMask, playerItemContainer);
                        UpdateItemList(targetContainer, targetItemMask, targetItemContainer);
                    }

                    break;
            }
        }

        #endregion

        #region Event Callbacks

        protected virtual void OnItemSelected(ContainerItem item) {
            selectedItem = item;
            currentTab = ReferenceEquals(item.Owner, targetContainer) ? EContainerTabs.Target : EContainerTabs.Player;

            UpdateDescriptionFields(item);
        }

        #endregion

        #region UI Functions

        protected virtual void UpdateDescriptionFields(ContainerItem itemData) {
            if (itemData.Model != null) {
                selectedItemNameLabel.text = itemData.Model.DisplayName;

                if (itemData.Quantity > 1) {
                    selectedItemNameLabel.text += $" (x{itemData.Quantity})";
                }

                if (itemData.IsEquipped) {
                    selectedItemNameLabel.text += $" (E)";
                }

                // TODO: Overhaul: IHasStatusEffects?
                selectedItemDescrLabel.enabled = false;

                if (itemData.Model is PotionData) {
                    PotionData potionData = (PotionData)itemData.Model;
                    selectedItemDescrLabel.text = "Effects:\n";

                    foreach (StatusEffectData statusEffectModel in potionData.StatusEffects) {
                        selectedItemDescrLabel.text += $"\t\t{statusEffectModel.BuffType.ToString()}\n";
                    }

                    selectedItemDescrLabel.enabled = true;
                }
            } else {
                selectedItemNameLabel.text = itemData.EquippedItem.DisplayName;

                if (itemData.EquippedItem.StackAmount > 1) {
                    selectedItemNameLabel.text += $" (x{itemData.Quantity})";
                }

                selectedItemNameLabel.text += $" (E)";

                selectedItemDescrLabel.enabled = false;
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
                targetContainer.Inventory.Add(itemData.Model.Id, itemData.Quantity);
                itemData.Owner.Inventory.Remove(itemData.Model.Id, itemData.Quantity);
            } else {
                // Remove and spawn the item
                itemData.Owner.Inventory.Remove(itemData.Model.Id, itemData.Quantity);
                IItem item = GameManager.SpawnAtPlayer(itemData.Model.Id) as IItem;

                // If the object is an item and it exists
                if (item != null) {
                    // Update its stack amount
                    item.StackAmount = itemData.Quantity;
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

            if (itemData.IsEquipped) {
                Actor.Unequip(actor, actor.Inventory, itemData.EquippedItem.EquipSlot);
            } else {
                if (itemData.Model is PotionData) {
                    PotionData potionData = (PotionData)itemData.Model;

                    actor.Inventory.Remove(itemData.Model.Id, 1);
                    
                    // TODO
                    //actor.Consume(potionData);
                } else if (itemData.Model is ArmorData || itemData.Model is WeaponData) {
                    // TODO
                    //EquippableModel equippable = (EquippableModel)itemData.Model;
                    //Actor.Equip(actor, actor.Inventory, equippable.id, itemData.Stack.Quantity);
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

        private void UpdateItemList(IContainer container, EContainerViewMask itemMask, Transform listContainer) {
            ContainerListButton lb;

            listContainer.DestroyAllChildren();

            if (itemMask.HasFlag(EContainerViewMask.All) || itemMask.HasFlag(EContainerViewMask.Equipped)) {
                if (container is Actor) {
                    Actor actor = (Actor)container;
                    EquippedItem[] eItems = actor.Equipment.Items.Where(x => x != null && x.EquippedObject != null).ToArray();

                    if (eItems.Length > 0) {
                        AddCategoryHeader(listContainer, EngineSettings.SettingHeaderEquipped);

                        foreach (EquippedItem item in eItems) {
                            CreateListButton(listContainer, container, item.EquippedObject);
                        }
                    }
                }
            }

            if (itemMask.HasFlag(EContainerViewMask.All) || itemMask.HasFlag(EContainerViewMask.Items)) {
                List<string> processedItems = new List<string>();

                foreach (string[] categoryPair in EngineSettings.ContainerCategories) {
                    List<InventoryEntry> iEntriesInCat = null;
                    if (!categoryPair[0].IsNullOrWhiteSpace()) {
                        iEntriesInCat = container.Inventory.Contents.Where(x => GameTag.HasTag(x.ItemData, categoryPair[0]) && !processedItems.Contains(x.Id)).ToList();
                    } else {
                        iEntriesInCat = container.Inventory.Contents.Where(x => x.ItemData.GameTags.Length == 0 && !processedItems.Contains(x.Id)).ToList();
                    }

                    if (iEntriesInCat.Count > 0) {
                        AddCategoryHeader(listContainer, categoryPair[1]);

                        foreach (InventoryEntry entry in iEntriesInCat) {
                            processedItems.Add(entry.Id);
                            CreateListButton(listContainer, container, entry);
                        }
                    }
                }
            }
        }

        private void AddCategoryHeader(Transform container, string categoryName) {
            GameObject spawnedObject = Instantiate(categoryHeaderPrefab, container, false);
            spawnedObject.GetComponentInChildren<Text>().text = categoryName.ToUpper();
        }

        private void CreateListButton(Transform listContainer, IContainer owner, InventoryEntry entry) {
            if (entry.Quantity > 1 && !entry.ItemData.IsStackable) {
                for (int i = 0; i < entry.Quantity; i++) {
                    AddButton(listContainer, owner, entry);
                }
            } else {
                AddButton(listContainer, owner, entry);
            }
        }

        private void CreateListButton(Transform listContainer, IContainer owner, IEquippable equippable) {
            AddButton(listContainer, owner, equippable);
        }

        private void AddButton(Transform container, IContainer owner, InventoryEntry entry) {
            GameObject spawnedObject = Instantiate(itemButtonPrefab, container, false);
            UpdateListButtonItem(spawnedObject.GetComponent<ContainerListButton>(), owner, entry.ItemData, entry.Quantity);
        }

        private void AddButton(Transform container, IContainer owner, IEquippable equippable) {
            GameObject spawnedObject = Instantiate(itemButtonPrefab, container, false);
            UpdateListButtonItem(spawnedObject.GetComponent<ContainerListButton>(), owner, equippable);
        }

        private void UpdateListButtonItem(ContainerListButton btn, IContainer owner, ItemData model, int quantity) {
            btn.SetItem(new ContainerItem(owner, model, quantity, false), OnItemSelected);
        }

        private void UpdateListButtonItem(ContainerListButton btn, IContainer owner, IEquippable equippable) {
            btn.SetItem(new ContainerItem(owner, equippable, true), OnItemSelected);
        }

        private void ClearSelectedItem() {
            selectedItem = null;

            selectedItemNameLabel.text = string.Empty;
            selectedItemDescrLabel.enabled = false;
        }

        #endregion

        public void View(IContainer source, IContainer target, EContainerViewMask sourceMask = EContainerViewMask.All, EContainerViewMask targetMask = EContainerViewMask.All) {
            if (source == null) {
                return;
            }

            sourceContainer = source;
            sourceItemMask = sourceMask;
            targetContainer = target;
            targetItemMask = targetMask;

            UpdateItemList(source, sourceItemMask, playerItemContainer);

            if (target != null) {
                targetItemListPanel?.SetActive(true);
                UpdateItemList(target, targetItemMask, targetItemContainer);
            }
            
            ClearSelectedItem();
            Show();
        }
    }
}