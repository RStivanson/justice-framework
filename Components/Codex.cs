using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using System;

namespace JusticeFramework.Components {
    [Serializable]
	public class Codex : Item {
		public override EInteractionType InteractionType {
			get { return EInteractionType.Read; }
		}

        protected override Logic.Action OnActivate(IWorldObject activator) {
            CodexData data = dataObject as CodexData;
            Logic.Action action = new ActionRead(this);
            action.Sound = data.PickupSound;
            return action;
		}
	}
}