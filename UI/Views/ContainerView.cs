using System;
using JusticeFramework.Components;
using JusticeFramework.Data.Collections;
using JusticeFramework.Data.Models;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.UI.Components;
using JusticeFramework.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    [Serializable]
    public class ContainerView : Window {
        private enum EContainerTabs {
            Player,
            Target            
        }

        private const KeyCode TRANSFER_KEY_CODE = KeyCode.R;
        private const KeyCode ACTIVATE_KEY_CODE = KeyCode.E;

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
        private int selectedItemIndex;
        private ItemModel selectedItemModel;
        private ItemListEntry selectedItemEntry;

#region Unity Events

        private void Update() {
            // If we do not have any item selected, return
            if (selectedItemModel == null) {
                return;
            }

            switch (currentTab) {
                case EContainerTabs.Player:
                    if (secondContainer != null && Input.GetKeyDown(TRANSFER_KEY_CODE)) {
                        TransferItem(firstContainer, secondContainer, selectedItemModel.id, firstContainer.Inventory[selectedItemIndex].count);
                        UpdateItemList(firstContainer, playerItemContainer);
                        UpdateItemList(secondContainer, targetItemContainer);
                    }

                    if (Input.GetKeyDown(ACTIVATE_KEY_CODE)) {
                        OnContainerViewItemActivated(firstContainer, selectedItemModel, 1);
                        UpdateItemList(firstContainer, playerItemContainer);
                    }

                    break;
                case EContainerTabs.Target:
                    if (Input.GetKeyDown(TRANSFER_KEY_CODE)) {
                        TransferItem(secondContainer, firstContainer, selectedItemModel.id, secondContainer.Inventory[selectedItemIndex].count);
                        UpdateItemList(firstContainer, playerItemContainer);
                        UpdateItemList(secondContainer, targetItemContainer);
                    }

                    break;
            }
        }

#endregion

#region Event Callbacks

        private void OnItemSelected(int index, IContainer owner, ItemListEntry entry, ItemModel item) {
            // Setup item info
            selectedItemIndex = index;
            selectedItemModel = item;
            selectedItemEntry = entry;

            currentTab = (owner == secondContainer) ? EContainerTabs.Target : EContainerTabs.Player;

            string itemName = selectedItemModel.displayName;

            if (selectedItemModel.isStackable) {
                itemName = $"{itemName} (x{owner.Inventory[index].count})";
            }
            
            selectedItemNameLabel.text = itemName;

            ConsumableModel consumable = selectedItemModel as ConsumableModel;
            selectedItemDescrLabel.enabled = consumable != null;
            selectedItemDescrLabel.text = $"Modifier: {consumable?.healthModifier}";
        }

#endregion

        public void TransferItem(IContainer owner, IContainer target, string id, int quantity) {
            if (string.IsNullOrEmpty(id)) {
                return;
            }
					
            // If there is a target for the item
            if (target != null) {
                // Transfer the item
                target.GiveItem(id, quantity);
                owner.TakeItem(id, quantity);
            } else { // If there is no target, then drop the item
                owner.TakeItem(id, quantity);
                GameManager.Spawn(id, transform.position + transform.forward * 5, Quaternion.identity);
            }
        }
        
        public static void OnContainerViewItemActivated(IContainer owner, ItemModel itemModel, int quantity) {
            if (owner is Actor) {
                Actor actor = (Actor)owner;

                if (itemModel is ConsumableModel) {
                    actor.Consume((ConsumableModel)itemModel);
                    actor.TakeItem(itemModel.id, 1);
                } else if (itemModel is EquippableModel) {
                    actor.Equip(GameManager.Spawn(itemModel.id, GameManager.Player.transform.position, Quaternion.identity) as IEquippable);
                    actor.TakeItem(itemModel.id, quantity);
                }
            }
        }
        
        private void UpdateItemList(IContainer container, Transform itemListContainer) {
            bool entryFound = false;
            itemListContainer.DestroyAllChildren();

            for (int i = 0; i < container.Inventory.Count; ++i) {
                int index = i;
                ItemListEntry entry = container.Inventory[i];
                ItemModel item = GameManager.AssetManager.GetEntityById<ItemModel>(entry.id);

                if (item == null) {
                    continue;
                }

                if (selectedItemEntry != null && ReferenceEquals(entry, selectedItemEntry)) {
                    OnItemSelected(index, container, entry, item);
                    entryFound = true;
                }
                
                if (container.Inventory[i].count > 1 && !item.isStackable) {
                    for (int k = 0; k < container.Inventory[i].count; ++k) {
                        AddButton(itemListContainer, container.Inventory[i], item, delegate {
                            OnItemSelected(index, container, entry, item);
                        });
                    }
                } else {
                    AddButton(itemListContainer, container.Inventory[i], item, delegate {
                        OnItemSelected(index, container, entry, item);
                    });
                }
            }

            if (!entryFound) {
                ClearSelectedItem();
            }
        }
        
        private void AddButton(Transform container, ItemListEntry item, ItemModel model, UnityAction callback) {
            GameObject spawnedObject = Instantiate(itemButtonPrefab);
            
            spawnedObject.GetComponent<Button>().onClick.AddListener(callback);
            spawnedObject.GetComponent<InventoryListButton>().SetItem(item, model);
            
            spawnedObject.transform.SetParent(container, false);
        }

        private void ClearSelectedItem() {
            selectedItemIndex = -1;
            selectedItemModel = null;
            selectedItemEntry = null;

            selectedItemNameLabel.text = string.Empty;
            selectedItemDescrLabel.enabled = false;
        }

        public void View(IContainer first, IContainer second) {
            if (first == null) {
                return;
            }

            firstContainer = first;
            secondContainer = second;

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