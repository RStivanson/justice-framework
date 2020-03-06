using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
	public class Item : WorldObject {
        [SerializeField]
        private int stackAmount;

        public override EInteractionType InteractionType {
			get { return EInteractionType.Take; }
		}

        public int StackAmount {
            get { return stackAmount; }
        }

        protected override Logic.Action OnActivate(IWorldObject activator) {
            ItemData data = dataObject as ItemData;
            return new ActionTake(this, data.PickupSound);
        }
    }
}