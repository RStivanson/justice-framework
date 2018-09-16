using JusticeFramework.UI.Views;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Components {
    /// <inheritdoc />
    /// <summary>
    /// Basic model container script attached to each button in a container screen list
    /// </summary>
    [Serializable]
	public class ContainerListButton : MonoBehaviour {
		/// <summary>
		/// The main button script on this object
		/// </summary>
		[SerializeField]
		private Button buttonScript;
		
		/// <summary>
		/// The item name GUI text component
		/// </summary>
		[SerializeField]
		private Text itemNameText;

		/// <summary>
		/// The item weight GUI text component
		/// </summary>
		[SerializeField]
		private Text itemWeightText;

		/// <summary>
		/// The item value GUI text component
		/// </summary>
		[SerializeField]
		private Text itemValueText;

		/// <summary>
		/// The reference to all container data
		/// </summary>
		[SerializeField]
		private ContainerView.ContainerItem containerItem;

        /// <summary>
        /// Gets the container data attached to this object
        /// </summary>
        public ContainerView.ContainerItem Item {
			get { return containerItem; }
		}

        /// <summary>
        /// Attaches the data to this object
        /// </summary>
        /// <param name="itemEntry">The container item data</param>
		/// <param name="showQuantity">Flag indicating if the quantity of the item should be shown</param>
        public void SetItem(ContainerView.ContainerItem item, OnContainerAction callback) {
            containerItem = item;

            if (callback != null) {
                buttonScript.onClick.AddListener(delegate {
                    callback(Item);
                });
            }

            RefreshFields();
		}

		/// <summary>
		/// Refreshes the text components with the stored model's values
		/// </summary>
		private void RefreshFields() {
            if (containerItem.Model == null && containerItem.EquippedItem == null) {
                itemNameText.text = "Missing item data...";
            } else {
                if (containerItem.Model != null) {
                    itemNameText.text = containerItem.Model.displayName;

                    if (containerItem.Stack.Quantity > 1) {
                        itemNameText.text += $" ({containerItem.Stack.Quantity})";
                    }

                    if (containerItem.IsEquipped) {
                        itemNameText.text += " (E)";
                    }

                    itemWeightText.text = containerItem.Model.weight.ToString();
                    itemValueText.text = containerItem.Model.value.ToString();
                } else {
                    itemNameText.text = containerItem.EquippedItem.DisplayName;

                    if (containerItem.EquippedItem.StackAmount > 1) {
                        itemNameText.text += $" ({containerItem.EquippedItem.StackAmount})";
                    }

                    itemNameText.text += " (E)";

                    itemWeightText.text = containerItem.EquippedItem.Weight.ToString();
                    itemValueText.text = containerItem.EquippedItem.Value.ToString();
                }
            }
		}
	}
}
