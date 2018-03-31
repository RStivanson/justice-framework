using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Data class for all harvestable items such as flowers, crops, ores, etc.
	/// </summary>
	[Serializable]
	public class FlowerModel : WorldObjectModel {
		/// <summary>
		/// Item data defining what is received when harvested
		/// </summary>
		public HarvestModel harvestData = null;

		/// <summary>
		/// A count of seconds for this to regrow after being harvested
		/// </summary>
		public int respawnTimeInSeconds = 10;
		
		/// <summary>
		/// Sound clip to be played upon harvesting
		/// </summary>
		public AudioClip harvestSound = null;
	}
}