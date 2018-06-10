using System;
using System.Collections.Generic;
using System.Linq;
using JusticeFramework.Core.Models.Quest;

namespace JusticeFramework.Core.Managers.Resources {
    [Serializable]
    public class QuestManager : ResourceManager<Quest> {
        private string DataPath = "Data/Quests";

        public override void LoadResources() {
            LoadResources(DataPath);
        }

        public List<Quest> GetNotStartedQuests(string id) {
            return resources.Where(quest => quest.QuestState == EQuestState.NotStarted).ToList();
        }

        public List<Quest> GetInProgressQuests(string id) {
            return resources.Where(quest => quest.QuestState == EQuestState.InProgress).ToList();
        }

        public List<Quest> GetCompletedQuests(string id) {
            return resources.Where(quest => quest.QuestState == EQuestState.Completed).ToList();
        }

        public List<Quest> GetFailedQuests(string id) {
            return resources.Where(quest => quest.QuestState == EQuestState.Failed).ToList();
        }

        public int GetQuestStage(string id) {
            return GetById(id)?.Marker ?? -1;
        }

        public EQuestState GetQuestState(string id) {
            return GetById(id)?.QuestState ?? EQuestState.Invalid;
        }

        public bool IsQuestAtStage(string id, int stageMarker) {
            return GetQuestStage(id) == stageMarker;
        }

        public bool IsQuestAtState(string id, EQuestState questState) {
            return GetQuestState(id) == questState;
        }

        public void SetQuestStage(string id, int marker) {
            Quest quest = GetById(id);

            if (quest != null) {
                quest.SetMarker(marker);
            }
        }
    }
}
