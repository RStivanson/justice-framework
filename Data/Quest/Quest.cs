using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Data.Quest {
	[Serializable]
	public class Quest : ScriptableObject {
		public string id; // System identifier
		public string name; // Display name
		public bool active;
		public List<QuestStage> stages; // Quest Components
	}
}