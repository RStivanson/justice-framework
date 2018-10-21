using System;
using System.Collections.Generic;
using System.Linq;
using JusticeFramework.Core.Models.Quest;
using JusticeFramework.Core.Models.Settings;

namespace JusticeFramework.Core.Managers.Resources {
    public delegate void OnQuestAction(string questId, int marker);

    [Serializable]
    public class QuestManager : ResourceManager<Quest> {
        public event OnQuestAction onQuestUpdated;

        private string DataPath = "Data/Quests";

        public override void LoadResources() {
            LoadResources(DataPath);
        }

        public List<Quest> GetNotStartedQuests() {
            return resources.Where(quest => quest.QuestState == EQuestState.NotStarted).ToList();
        }

        public List<Quest> GetInProgressQuests() {
            return resources.Where(quest => quest.QuestState == EQuestState.InProgress).ToList();
        }

        public List<Quest> GetCompletedQuests() {
            return resources.Where(quest => quest.QuestState == EQuestState.Completed).ToList();
        }

        public List<Quest> GetFailedQuests() {
            return resources.Where(quest => quest.QuestState == EQuestState.Failed).ToList();
        }

        public string GetQuestDisplayName(string id) {
            return GetById(id)?.DisplayName ?? SystemConstants.LabelUnknown;
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

        public void SetQuestStage(string id, int marker, bool silent = false) {
            Quest quest = GetById(id);

            if (quest != null) {
                quest.SetMarker(marker);

                if (!silent) {
                    onQuestUpdated?.Invoke(id, marker);
                }
            }
        }
    }
}
