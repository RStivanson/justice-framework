using JusticeFramework.Collections;
using JusticeFramework.Console;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using JusticeFramework.Managers;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
	public class Chest : WorldObject, IContainer, ILockable {
		/// <summary>
		/// The inventory of items in this container.
		/// </summary>
		[SerializeField]
		private Inventory inventory;

		/// <summary>
		/// Flag indicating if this chest is locked.
		/// </summary>
		[SerializeField]
		private bool isLocked;

		/// <summary>
		/// The difficulty of the lock on this chest.
		/// </summary>
		[SerializeField]
		private ELockDifficulty lockDifficulty;

        /// <summary>
        /// The key required to unlock this chest.
        /// </summary>
        [SerializeField]
        private ItemData requiredKey;

        public override string DisplayName {
            get {
                string displayName = base.DisplayName;

                if (IsLocked) {
                    displayName = $"{displayName} ({LockDifficulty.ToString()})";
                }

                return displayName;
            }
        }

        public override EInteractionType InteractionType {
			get { return EInteractionType.Loot; }
		}
		
		public Inventory Inventory {
			get { return inventory; }
		}
		
		public bool IsLocked {
			get { return isLocked; }
		}
		
		public ELockDifficulty LockDifficulty {
			get { return lockDifficulty; }
		}

        public ItemData RequiredKey {
            get { return requiredKey; }
        }

        protected override void OnIntialized() {
            ResetToDefaultItems();
        }

        public void ResetToDefaultItems() {
            ChestData data = dataObject as ChestData;

            Inventory.Clear();

            foreach (ItemStackData isd in data.DefaultInventory) {
                Inventory.Add(isd.itemData.Id, isd.quantity);
            }
        }

		[ConsoleCommand("giveitem", "Gives the chest the item with the given id and quantity", ECommandTarget.LookAt)]
		private void GiveItem(string id, int amount) {
			if (!GameManager.DataManager.IsAssetLoaded<ItemData>(id)) {
				return;
			}

			Inventory.Add(id, amount);
		}
		
        public void ActivateItem(string id) {
			Debug.Log($"Container cannot activate items. (name: {name}, id: {id})");
		}
		
		[ConsoleCommand("lock", "Locks the target container", ECommandTarget.LookAt)]
		public void Lock() {
			isLocked = true;
            // TODO
            // PlaySound(LockSound, EAudioType.SoundEffect, 1);
            OnReferenceStateChanged();
		}
		
		[ConsoleCommand("unlock", "Unlocks the target container", ECommandTarget.LookAt)]
		public void Unlock() {
			isLocked = false;
            // TODO
            // PlaySound(LockSound, EAudioType.SoundEffect, 1);
            OnReferenceStateChanged();
		}

        protected override Logic.Action OnActivate(IWorldObject activator) {
            Logic.Action action = null;

            if (IsLocked) {
                if (LockDifficulty == ELockDifficulty.Impossible && requiredKey == null) {
                    action = new ActionFailed(this, "This chest cannot be unlocked.");
                    return action;
                }

                IContainer container = activator as IContainer;
                if (container.Inventory.Contains(requiredKey.Id)) {
                    Unlock();
                } else if (container.Inventory.HasItemWithTag("ItemLockpick")) {
                    // TODO
                    // action = new ActionLockpick(this);
                    // return;
                }
            }

            if (!IsLocked) {
                action = new ActionOpen(this);
                return action;
            }

            return new ActionFailed(this, null);
		}
	}
}