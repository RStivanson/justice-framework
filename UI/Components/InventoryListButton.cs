using System;
using System.Globalization;
using JusticeFramework.Core.Collections;
using JusticeFramework.Core.Models;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Components {
	/// <inheritdoc />
	/// <summary>
	/// Basic model container script attached to each button in the inventory screen list
	/// </summary>
	[Serializable]
	[RequireComponent(typeof(Button))]
	public class InventoryListButton : MonoBehaviour {
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
		/// The item list entry as stored in the inventory
		/// </summary>
		[SerializeField]
		private ItemListEntry itemListEntry;
		
		/// <summary>
		/// The item model this button relates to
		/// </summary>
		[SerializeField]
		private ItemModel itemModel;

		/// <summary>
		/// Flag indicating if the quantity of the item should be shown on the button
		/// </summary>
		[SerializeField]
		private bool shouldShowQuantity;

		/// <summary>
		/// Gets the item model attached to this object
		/// </summary>
		public ItemModel Model {
			get { return itemModel; }
		}
		
		/// <summary>
		/// Attaches the model to this object
		/// </summary>
		/// <param name="itemEntry">The Inventory item entry of the item</param>
		/// <param name="model">The model to attach to this object</param>
		/// <param name="showQuantity">Flag indicating if the quantity of the item should be shown</param>
		public void SetItem(ItemListEntry itemEntry, ItemModel model, bool showQuantity = true) {
			itemListEntry = itemEntry;
			itemModel = model;
			shouldShowQuantity = showQuantity;
			
			RefreshFields();
		}

		/// <summary>
		/// Refreshes the text components with the stored model's values
		/// </summary>
		private void RefreshFields() {
			if (itemListEntry == null || itemModel == null) {
				return;
			}

			itemNameText.text = shouldShowQuantity ? 
								$"{itemModel.displayName} (x{itemListEntry.count})" : 
								itemModel.displayName;

			itemWeightText.text = itemModel.weight.ToString(CultureInfo.InvariantCulture);
			itemValueText.text = itemModel.value.ToString();
		}
	}
}
