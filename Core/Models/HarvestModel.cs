using System;

namespace JusticeFramework.Core.Models {
	/// <summary>
	/// Model class that defines harvest information needed when harvest a flower object
	/// </summary>
	[Serializable]
	public class HarvestModel {
		/// <summary>
		/// The ID of the item to receive on harvest
		/// </summary>
		public string id = string.Empty;
		
		/// <summary>
		/// The quantity of item given from a harvest
		/// </summary>
		public int quantity = 0;
	}
}