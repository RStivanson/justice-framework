using System;
using System.Collections.Generic;
using System.Linq;
using JusticeFramework.Data.Quest;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework {
	[Serializable]
	public class QuestManager : IOnProgressChanged {
		[SerializeField]
		private bool initialized = false;

		[SerializeField]
		private Quest[] quests;
		
		[SerializeField]
		private Dictionary<string, Quest> questsById;

		public event OnProgressChanged onProgressChanged;

		public int Count() {
			return quests.Length;
		}

		public void Initialize() {
			if (initialized) {
				return;
			}

			initialized = true;

			if (onProgressChanged != null) {
				onProgressChanged(false, 0, "Loading quests");
			}
			
			quests = Resources.LoadAll<Quest>("Data/Quests");
			questsById = new Dictionary<string, Quest>();
			
			for (int i = 0; i < quests.Length; ++i) {
				onProgressChanged?.Invoke(false, 0.2f + (0.8f * (i / (float)quests.Length)), "Post-processing quest: " + quests[i].id);

				questsById.Add(quests[i].id, quests[i]);
			}

			onProgressChanged?.Invoke(true, 1.0f, "Done");
		}

		public Quest GetQuests(string id) {
			Quest quest;
			
			questsById.TryGetValue(id, out quest);

			return quest;
		}

		// Optimize.. On Active Changed event? Add to Dictionary?
		public List<Quest> GetActiveQuests(string id) {
			return quests.Where(quest => quest.active).ToList();
		}
	}
}
