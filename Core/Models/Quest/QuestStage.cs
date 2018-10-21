using System;

namespace JusticeFramework.Core.Models.Quest {
	[Serializable]
	public class QuestStage {
		public int marker; // Marker to distinguish the stage inside a quest
		public string logEntry; // Text to show on the quest log
		public bool completeQuest; // Flag stating if this stage completes the quest
		public bool failQuest; // Flag stating if this stage fails the quest
        public bool completed; // Flag indicated if this stage was completed
	}
}