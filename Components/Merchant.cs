using System;
using JusticeFramework.Core;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
    public class Merchant : MonoBehaviour {
        [SerializeField]
        private MerchantInventory merchantInventory;

        public MerchantInventory Inventory {
            get { return merchantInventory;  }
        }

        private void Awake() {
            // Clone the inventory to not affect the base data
            merchantInventory = Instantiate(merchantInventory);
        }
    }
}