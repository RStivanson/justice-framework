using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	[Serializable]
	[Obsolete("ID and prefab combo can be obtained through the WorldObjectModel class")]
	public class ReferenceDefinition : ScriptableObject {
		public string id = string.Empty;
		public GameObject prefab = null;
	}
}