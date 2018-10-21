using JusticeFramework.Components;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models.Quest;
using JusticeFramework.Core.UI;
using JusticeFramework.UI.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    public delegate void OnQuestAction(Quest quest);

    [Serializable]
    public class QuestView : Window {
        [SerializeField]
        private Text selectedItemNameLabel;

        [SerializeField]
        private Text selectedItemDescrLabel;

        [SerializeField]
        private Transform buttonContainer;

        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private Actor currentTarget;

        [SerializeField]
        [HideInInspector]
        private List<Quest> inProgressQuests;

        [SerializeField]
        private Quest selectedQuest;

        private void UpdateDescriptionFields(Quest quest) {
            string description = string.Empty;
            int counter = 0;

            for (int i = 0; i < quest.Stages.Count; i++) {
                if (!quest.Stages[i].completed && quest.Stages[i].marker != quest.Marker) {
                    continue;
                }

                counter++;
                description += $"{counter}. {quest.Stages[i].logEntry}";

                if (i < quest.Stages.Count - 1) {
                    description += Environment.NewLine;
                }
            }

            selectedItemNameLabel.text = quest.DisplayName;
            selectedItemDescrLabel.text = description;
        }

        protected override void OnShow() {
            inProgressQuests = GameManager.QuestManager.GetInProgressQuests();

            PopulateQuestList();
        }

        #region Quest List

        private void PopulateQuestList() {
            if (inProgressQuests == null) {
                return;
            }

            foreach (Quest quest in inProgressQuests) {
                AddButton(buttonContainer, quest);
            }
        }

        private void AddButton(Transform container, Quest quest) {
            GameObject spawnedObject = Instantiate(buttonPrefab);

            spawnedObject.GetComponent<QuestListButton>().SetQuest(quest, OnQuestSelected);

            spawnedObject.transform.SetParent(container, false);
        }

        #endregion

        #region Event Callbacks

        protected virtual void OnQuestSelected(Quest quest) {
            selectedQuest = quest;
            UpdateDescriptionFields(quest);
        }

        #endregion
    }
}
