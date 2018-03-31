using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Base class for all game entities
	/// </summary>
	[Serializable]
	public class EntityModel : ScriptableObject {
		/// <summary>
		/// The ID of the entity
		/// </summary>
		public string id = string.Empty;

		/// <summary>
		/// A hashed version of the ID
		/// </summary>
		public int hashedId = string.Empty.GetHashCode();
		
		/// <summary>
		/// Method called when the Unity inspector updates the object
		/// </summary>
		protected virtual void OnValidate() {
			hashedId = id.GetHashCode();
		}
	}
}