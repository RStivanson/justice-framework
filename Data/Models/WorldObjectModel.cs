using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Base class for all spawned game entities including actors, items, and static objects.
	/// </summary>
	[Serializable]
	public class WorldObjectModel : EntityModel {
		/// <summary>
		/// The in-game display name
		/// </summary>
		public string displayName = "WorldObject";

		/// <summary>
		/// Flag indicating if the object can be activated
		/// </summary>
		public bool canBeActivated = true;
		
		/// <summary>
		/// Prefab model used when spawning this model
		/// </summary>
		public GameObject prefab = null;
	}
}