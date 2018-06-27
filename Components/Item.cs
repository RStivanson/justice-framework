using JusticeFramework.Core;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Models;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
	public class Item : WorldObject, IItem {
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