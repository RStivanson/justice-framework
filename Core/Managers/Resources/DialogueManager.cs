using JusticeFramework.Core.Models.Dialogue;
using JusticeFramework.Utility.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Managers.Resources {
    [Serializable]
    public class DialogueManager : ResourceManager<Conversation> {
        private const string DataPath = "Data/Dialogue/";

        [SerializeField]
        private Dictionary<string, List<Conversation>> factionDialogueByTargetId;

        [SerializeField]
        private Dictionary<string, List<Conversation>> dialogueByTargetId;

        public DialogueManager() : base() {
            factionDialogueByTargetId = new Dictionary<string, List<Conversation>>();
            dialogueByTargetId = new Dictionary<string, List<Conversation>>();
        }

        public override void LoadResources() {
            LoadResources(DataPath);
        }

        protected override void OnPreProcess() {
            factionDialogueByTargetId.Clear();
            dialogueByTargetId.Clear();
        }

        protected override void OnResourceProcessed(Conversation conversation) {
            if (conversation.isFactionDialogue) {
                factionDialogueByTargetId.AddToList(conversation.targetId, conversation);
            } else {
                dialogueByTargetId.AddToList(conversation.targetId, conversation);
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
