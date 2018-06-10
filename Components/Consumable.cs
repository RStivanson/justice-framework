using System;
using JusticeFramework.Core.Models;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Utility.Extensions;
using JusticeFramework.Core.Events;

namespace JusticeFramework.Components {
	[Serializable]
	public class Consumable : Item, IConsumable {
#region Properties

		private ConsumableModel ConsumableModel {
			get { return model as ConsumableModel; }
		}

#endregion
    }
}