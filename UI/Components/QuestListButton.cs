using JusticeFramework.Data;
using JusticeFramework.Logic;
using JusticeFramework.UI.Views;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Components {
    /// <inheritdoc />
    /// <summary>
    /// Basic quest information script attached to each button in a quest screen list
    /// </summary>
    [Serializable]
    public class QuestListButton : MonoBehaviour {
        /// <summary>
        /// The main button script on this object
        /// </summary>
        [SerializeField]
        private Button buttonScript;

        /// <summary>
        /// The item name GUI text component
        /// </summary>
        [SerializeField]
        private Text nameText;

        /// <summary>
        /// The reference to quest data
        /// </summary>
        [SerializeField]
        private QuestSequence quest;

        /// <summary>
        /// Gets the quest attached to this object
        /// </summary>
        public QuestSequence Quest {
            get { return quest; }
        }

        /// <summary>
        /// Attaches the data to this object
        /// </summary>
        /// <param name="quest">The quest data</param>
		/// <param name="callback">The event to call when the button is clicked</param>
        public void SetQuest(QuestSequence quest, OnQuestAction callback) {
            this.quest = quest;

            if (callback != null) {
                buttonScript.onClick.AddListener(delegate {
                    callback(quest);
                });
            }

            RefreshFields();
        }

        /// <summary>
        /// Refreshes the text components with the stored quests values
        /// </summary>
        private void RefreshFields() {
            if (quest == null) {
                nameText.text = "Missing quest data...";
            } else {
                nameText.text = quest.QuestData.DisplayName;
            }
        }
    }
}
