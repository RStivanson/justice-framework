using JusticeFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JusticeFramework.Logic {
    /// <summary>
    /// Runtime quest structure. This class handles the quest data and quest state.
    /// </summary>
    [Serializable]
    public class QuestSequence {
        /// <summary>
        /// Runtime quest stage structure.
        /// </summary>
        [Serializable]
        public class QuestStage {
            /// <summary>
            /// The data this quest stage represents.
            /// </summary>
            public QuestData.QuestStage stageData;

            /// <summary>
            /// Flag indicating if this stage has been compeleted;
            /// </summary>
            public bool isCompleted;

            public QuestStage(QuestData.QuestStage questStageData) {
                stageData = questStageData;
                isCompleted = false;
            }
        }

        /// <summary>
        /// The quest that this tree represents.
        /// </summary>
        private QuestData questData;

        /// <summary>
        /// The current state of the quest.
        /// </summary>
        private EQuestState questState = EQuestState.NotStarted;

        /// <summary>
        /// The marker of the stage this quest is currently at.
        /// </summary>
        private int currentMarker;

        /// <summary>
        /// A collection of runtime quest stage objects
        /// </summary>
        private QuestStage[] stages;

        /// <summary>
        /// Gets the quest's associated ID.
        /// </summary>
        public string Id {
            get { return questData.Id; }
        }

        /// <summary>
        /// Gets the current state of this quest.
        /// </summary>
        public EQuestState QuestState {
            get { return questState; }
        }

        /// <summary>
        /// Gets the current stage marker for this quest.
        /// </summary>
        public int CurrentMarker {
            get { return currentMarker; }
        }

        /// <summary>
        /// Gets the current state of this quest.
        /// </summary>
        public QuestData QuestData {
            get { return questData; }
        }

        public QuestSequence(QuestData questData) {
            this.questData = questData;

            stages = new QuestStage[questData.Stages.Length];
            for (int i = 0; i < questData.Stages.Length; i++) {
                stages[i] = new QuestStage(questData.Stages[i]);
            }

            Reset();
        }

        /// <summary>
        /// Rests this quest back to stage 0.
        /// </summary>
        public void Reset() {
            currentMarker = 0;
        }

        /// <summary>
        /// Gets the current stage this quest is currently pointing at.
        /// </summary>
        /// <returns>Returns the current quest stage.</returns>
        public QuestStage GetCurrentStage() {
            return stages[currentMarker];
        }

        /// <summary>
        /// Sets the quest's current stage to the supplied marker.
        /// </summary>
        /// <param name="newStage">The new quest stage marker.</param>
        public void SetStage(int newStage) {
            currentMarker = newStage;
        }

        /// <summary>
        /// Marks the current stage as complete and updates the quest state.
        /// </summary>
        public void CompleteCurrentStage() {
            QuestStage stage = GetCurrentStage();

            if (stage != null) {
                stage.isCompleted = true;

                if (stage.stageData.completesQuest) {
                    Debug.Log(questData.DisplayName + " quest is now complete!");
                    questState = EQuestState.Completed;
                } else if (stage.stageData.failsQuest) {
                    Debug.Log(questData.DisplayName + " quest has failed!");
                    questState = EQuestState.Failed;
                } else if (currentMarker != 0) {
                    Debug.Log(questData.DisplayName + " quest is now in progress!");
                    questState = EQuestState.InProgress;
                }
            }
        }

        /// <summary>
        /// Determines if this quest has a stage matching the given marker.
        /// </summary>
        /// <param name="marker">The stage marker</param>
        /// <returns>Returns true if a stage shares the given marker, false otherwise.</returns>
        public bool HasStage(int marker) {
            return stages.Any(x => x.stageData.marker == marker);
        }

        public IEnumerable<QuestStage> GetActiveStages() {
            return stages.Where(x => x.isCompleted || x.stageData.marker == currentMarker);
        }
    }
}
