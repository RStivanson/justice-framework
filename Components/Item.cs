using System;
using JusticeFramework.Data.Models;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using UnityEngine;

namespace JusticeFramework.Components {
	[Serializable]
	public class Item : Reference, IItem {
#region Properties

		private ItemModel ItemModel {
			get { return model as ItemModel; }
		}
		
		public override EInteractionType InteractionType {
			get { return EInteractionType.Take; }
		}
		
		public int Value {
			get { return ItemModel.value; }
		}
		
		public float Weight {
			get { return ItemModel.weight; }
		}
		
		public bool IsStackable {
			get { return ItemModel.isStackable; }
		}
		
		public int MaxStackAmount {
			get { return ItemModel.MAX_STACK_AMOUNT; }
		}
		
		public AudioClip PickupSound {
			get { return ItemModel.pickupSound; }
		}

		public AudioClip DropSound {
			get { return ItemModel.dropSound; }
		}
		
#endregion
		
		public override void Activate(object sender, ActivateEventArgs e) {
			if (e?.Activator != null) {
				return;
			}

			if (e?.ActivatedBy is IContainer) {
				IContainer container = (IContainer)e?.ActivatedBy;
				container.GiveItem(ItemModel.id, 1);
				Destroy(gameObject);
			}
		}
	}
}