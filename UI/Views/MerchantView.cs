using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Models.Settings;
using System;

namespace JusticeFramework.UI.Views {
    [Serializable]
    public class MerchantView : ContainerView {
        protected override bool CanTransferItem(ContainerItem itemData, IContainer target) {
            return target.Inventory.Contains(SystemConstants.ItemCurrencyId, itemData.Model.value * itemData.Stack.Quantity);
        }

        protected override void OnItemTransfered(ContainerItem itemData, IContainer targetContainer) {
            targetContainer.Inventory.Remove(SystemConstants.ItemCurrencyId, itemData.Model.value * itemData.Stack.Quantity);
        }

        protected override bool CanActivateItem(ContainerItem itemData, IContainer targetContainer) {
            return false;
        }
    }
}