using JusticeFramework.Interfaces;
using JusticeFramework.Models.Settings;
using System;

namespace JusticeFramework.UI.Views {
    [Serializable]
    public class MerchantView : ContainerView {
        protected override bool CanTransferItem(ContainerItem itemData, IContainer target) {
            return target.Inventory.HasItemWithTag("ItemCurrency", itemData.Model.Value * itemData.Quantity);
        }

        protected override void OnItemTransfered(ContainerItem itemData, IContainer targetContainer) {
            targetContainer.Inventory.HasItemWithTag("ItemCurrency", itemData.Model.Value * itemData.Quantity);
        }

        protected override bool CanActivateItem(ContainerItem itemData, IContainer targetContainer) {
            return false;
        }
    }
}