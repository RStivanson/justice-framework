using JusticeFramework.Interfaces;
using System;
using System.Linq;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Data class for all quests.
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Quest Data", order = 102)]
    [Serializable]
    public class QuestData : ScriptableDataObject, IDisplayable {
        /// <summary>
        /// Data class that defines each individual stages of a quest. eg. "Go talk to the farmer", "Collect 15 hoofs"
        /// </summary>
        [Serializable]
        public class QuestStage {
            /// <summary>
            /// Marker that distinguishes different stages in quest. Very similar to an ID.
            /// </summary>
            public int marker;

            /// <summary>
            /// Text displayed in the quest log.
            /// </summary>
            public string logEntry;

            /// <summary>
            /// Flag indicating if this stage causes the quest to complete.
            /// </summary>
            public bool completesQuest;

            /// <summary>
            /// Flag indicating if this stage causes the quest to fail.
            /// </summary>
            public bool failsQuest;
        }

        /// <summary>
        /// The name to be displayed in-game
        /// </summary>
        [SerializeField]
        private string displayName; // Display name

        /// <summary>
        /// Flag indicating if this quest should be shown in the quest log.
        /// </summary>
        [SerializeField]
        private bool hiddenFromPlayer;

        /// <summary>
        /// The stages associated with this quest.
        /// </summary>
        [SerializeField]
        private QuestStage[] stages;

        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        public string DisplayName {
            get { return displayName; }
        }

        /// <summary>
        /// Gets all stages in this quest.
        /// </summary>
        public QuestStage[] Stages {
            get { return stages; }
        }

        /// <summary>
        /// Gets if this quest should be hidden from the player.
        /// </summary>
        public bool HiddenFromPlayer {
            get { return hiddenFromPlayer; }
        }
    }
}