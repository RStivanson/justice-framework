using JusticeFramework.Core.Collections;
using JusticeFramework.Core.Interfaces;
using System;
using UnityEngine;

namespace JusticeFramework.Core {
    /// <inheritdoc />
    /// <summary>
    /// Model class for inventories used by merchants.
    /// </summary>
    [Serializable]
    public class MerchantInventory : ScriptableObject, IContainer {
        /// <summary>
        /// The inventory available to the merchant
        /// </summary>
        public Inventory inventory;

        public Inventory Inventory {
            get { return inventory; }
        }

        public int GetQuantity(string id) {
            return (inventory[id] != null ? inventory[id].count : 0);
        }

        public void GiveItem(string id, int quantity) {
            inventory.AddItem(id, quantity, 0);
        }

        public bool TakeItem(string id, int quantity) {
            return inventory.RemoveItem(id, quantity) != 0;
        }
    }
}