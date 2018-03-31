using System;
using System.Collections.Generic;
using JusticeFramework.Data.Dialogue;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework {
	[Serializable]
	public class DialogueManager : IOnProgressChanged {
		[SerializeField]
		private bool initialized = false;

		[SerializeField]
		private Conversation[] conversations;
		
		[SerializeField]
		private Dictionary<string, List<Conversation>> factionDialogueByTargetId;

		[SerializeField]
		private Dictionary<string, List<Conversation>> dialogueByTargetId;

		public event OnProgressChanged onProgressChanged;

		public int Count() {
			return conversations.Length;
		}

		public void Initialize() {
			if (initialized) {
				return;
			}

			initialized = true;

			if (onProgressChanged != null) {
				onProgressChanged(false, 0, "Loading actor factions");
			}
			
			conversations = Resources.LoadAll<Conversation>("Data/Dialogue");
			factionDialogueByTargetId = new Dictionary<string, List<Conversation>>();
			dialogueByTargetId = new Dictionary<string, List<Conversation>>();
			
			for (int i = 0; i < conversations.Length; ++i) {
				if (onProgressChanged != null) {
					onProgressChanged(false, 0.2f + (0.8f * (i / (float)conversations.Length)), "Post-processing faction: " + conversations[i].Id);
				}

				List<Conversation> tempList = new List<Conversation>();

				if (conversations[i].isFactionDialogue) {
					if (factionDialogueByTargetId.TryGetValue(conversations[i].targetId, out tempList)) {
						tempList.Add(conversations[i]);
					} else {
						tempList = new List<Conversation> {
							conversations[i]
						};

						factionDialogueByTargetId.Add(conversations[i].targetId, tempList);
					}
				} else {
					if (dialogueByTargetId.TryGetValue(conversations[i].targetId, out tempList)) {
						tempList.Add(conversations[i]);
					} else {
						tempList = new List<Conversation> {
							conversations[i]
						};

						dialogueByTargetId.Add(conversations[i].targetId, tempList);
					}
				}
			}

			if (onProgressChanged != null) {
				onProgressChanged(true, 1.0f, "Done");
			}
		}

		public List<Conversation> GetDialogue(string id) {
			List<Conversation> tempList;
			
			dialogueByTargetId.TryGetValue(id, out tempList);

			return tempList;
		}

		public List<Conversation> GetFactionDialogue(string id) {
			List<Conversation> tempList;
			
			factionDialogueByTargetId.TryGetValue(id, out tempList);

			return tempList;
		}
	}
}
