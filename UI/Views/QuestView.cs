using JusticeFramework.Components;
using JusticeFramework.Core.UI;
using JusticeFramework.Logic;
using JusticeFramework.Managers;
using JusticeFramework.UI.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    public delegate void OnQuestAction(QuestSequence quest);

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
        private List<QuestSequence> inProgressQuests;

        [SerializeField]
        private QuestSequence selectedQuest;

        private void UpdateDescriptionFields(QuestSequence quest) {
            string description = string.Empty;
            int counter = 1;

            IEnumerable<QuestSequence.QuestStage> stages = quest.GetActiveStages();

            foreach (QuestSequence.QuestStage stage in stages) {
                description += $"{counter}. {stage.stageData.logEntry}";
                description += Environment.NewLine;

                counter++;
            }

            selectedItemNameLabel.text = quest.QuestData.DisplayName;
            selectedItemDescrLabel.text = description;
        }

        protected override void OnShow() {
            inProgressQuests = new List<QuestSequence>();//GameManager.DataManager.GetInProgressQuests();

            PopulateQuestList();
        }

        #region Quest List

        private void PopulateQuestList() {
            if (inProgressQuests == null) {
                return;
            }

            foreach (QuestSequence quest in inProgressQuests) {
                AddButton(buttonContainer, quest);
            }
        }

        private void AddButton(Transform container, QuestSequence quest) {
            GameObject spawnedObject = Instantiate(buttonPrefab);

            spawnedObject.GetComponent<QuestListButton>().SetQuest(quest, OnQuestSelected);

            spawnedObject.transform.SetParent(container, false);
        }

        #endregion

        #region Event Callbacks

        protected virtual void OnQuestSelected(QuestSequence quest) {
            selectedQuest = quest;
            UpdateDescriptionFields(quest);
        }

        #endregion
    }
}
