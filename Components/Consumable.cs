using System;
using JusticeFramework.Data.Models;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Utility.Extensions;

namespace JusticeFramework.Components {
	[Serializable]
	public class Consumable : Item, IConsumable {
#region Properties

		private ConsumableModel ConsumableModel {
			get { return model as ConsumableModel; }
		}
		
		public int HealthModifier {
			get { return ConsumableModel.healthModifier; }
		}
		
#endregion
	}
}