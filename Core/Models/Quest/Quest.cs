using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Models.Quest {
	[Serializable]
	public class Quest : EntityModel {
        [SerializeField]
        private new string name; // Display name

        [SerializeField]
        private EQuestState questState = EQuestState.NotStarted;

        [SerializeField]
        private int currentMarker;

        [SerializeField]
		private List<QuestStage> stages; // Quest Components

        [SerializeField]
        private bool hiddenFromPlayer;

        public string Id {
            get { return id; }
            set { id = value; }
        }

        public string DisplayName {
            get { return name; }
            set { name = value; }
        }

        public EQuestState QuestState {
            get { return questState; }
            set { questState = value; }
        }

        public int Marker {
            get { return currentMarker; }
            set { currentMarker = value; }
        }

        public List<QuestStage> Stages {
            get { return stages; }
            set { stages = value; }
        }

        public bool HiddenFromPlayer {
            get { return hiddenFromPlayer; }
            set { hiddenFromPlayer = value; }
        }

        public void SetMarker(int marker) {
            QuestStage currentStage = GetStageByMarker(currentMarker);

            if (currentStage != null) {
                currentStage.completed = true;
            }

            currentMarker = marker;

            QuestStage stage = GetStageByMarker(currentMarker);
            if (stage != null) {
                if (stage.completeQuest) {
                    Debug.Log(name + " quest is now complete!");
                    questState = EQuestState.Completed;
                } else if (stage.failQuest) {
                    Debug.Log(name + " quest has been failed!");
                    questState = EQuestState.Failed;
                } else if (marker != 0) {
                    Debug.Log(name + " quest is now in progress!");
                    questState = EQuestState.InProgress;
                }
            }
        }

        public QuestStage GetStageByMarker(int marker) {
            QuestStage stage = null;

            for (int i = 0; i < stages.Count && stage == null; i++) {
                if (stages[i].marker == marker) {
                    stage = stages[i];
                }
            }

            return stage;
        }
	}
}