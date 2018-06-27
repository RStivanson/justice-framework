using JusticeFramework.Core;
using JusticeFramework.Core.Collections;
using JusticeFramework.Core.Console;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models;
using JusticeFramework.Interfaces;
using JusticeFramework.UI.Views;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
	public class Chest : WorldObject, IChest, IInventory, IInteractable, ILockable {
		public event OnItemAdded onItemAdded;
		public event OnItemRemoved onItemRemoved;

#region Variables

		/// <summary>
		/// The inventory of items in this container
		/// </summary>
		[SerializeField]
		private Inventory inventory;

		/// <summary>
		/// Flag indicating if this chest is locked
		/// </summary>
		[SerializeField]
		private bool isLocked;

		/// <summary>
		/// The difficulty of the lock
		/// </summary>
		[SerializeField]
		private ELockDifficulty lockDifficulty;

#endregion
		
#region Properties

		private ChestModel ChestModel {
			get { return model as ChestModel; }
		}
		
		public override string DisplayName {
			get {
				string displayName = ChestModel.displayName;

				if (IsLocked) {
					displayName = $"{displayName} ({LockDifficulty.ToString()})";
				}

				return displayName;
			}
		}
		
		public EInteractionType InteractionType {
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

		public AudioClip OpenSound {
			get { return ChestModel.openSound; }
		}
		
#endregion

		[ConsoleCommand("giveitem", "Gives the chest the item with the given id and quantity", ECommandTarget.LookAt)]
		public void GiveItem(string id, int amount) {
			ItemModel item = GameManager.AssetManager.GetById<ItemModel>(id);

			if (item == null) {
				return;
			}

			Inventory.AddItem(id, amount, item.weight);
			onItemAdded?.Invoke(this, id, amount);
		}
		
		public bool TakeItem(string id, int amount) {
			int removed = Inventory.RemoveItem(id, amount);

			if (removed != 0) {
				onItemRemoved?.Invoke(this, id, removed);
			}

            return removed != 0;
		}

        public int GetQuantity(string id) {
            ItemListEntry entry = Inventory[id];
            return entry?.count ?? 0;
        }

        public void ActivateItem(string id) {
			Debug.Log($"Container cannot activate items. (name: {name}, id: {id})");
		}
		
		[ConsoleCommand("lock", "Locks the target container", ECommandTarget.LookAt)]
		public void Lock() {
			isLocked = true;
			OnReferenceStateChanged();
		}
		
		[ConsoleCommand("unlock", "Unlocks the target container", ECommandTarget.LookAt)]
		public void Unlock() {
			isLocked = false;
			OnReferenceStateChanged();
		}

		private void ToggleLock() {
			isLocked = !isLocked;
			OnReferenceStateChanged();
		}

		public override void Activate(object sender, ActivateEventArgs e) {
			if (e?.Activator != null) {
				ToggleLock();
			} else if (ReferenceEquals(GameManager.Player, e?.ActivatedBy)) {
				if (!IsLocked) {
					ContainerView view = UiManager.UI.OpenWindow<ContainerView>();
					view.View(e?.ActivatedBy as Actor, this);
				} else {
					Debug.Log("This needs picklocked");
				}
			}
		}
	}
}