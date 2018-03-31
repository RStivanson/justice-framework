using System;

namespace JusticeFramework.Data.Quest {
	[Serializable]
	public class QuestStage {
		public int marker; // Marker to distinguish where an actor is on this quest
		public string logEntry; // Text to show on the quest log
		public bool completeQuest; // Flag stating if this stage completes the quest
		public bool failQuest; // Flag stating if this stage fails the quest
	}
}