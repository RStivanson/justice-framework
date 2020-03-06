using System;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Defines a player's dialogue response that can be chosen by the player.
    /// </summary>
    [Serializable]
    public class DialogueTopicData {
        /// <summary>
        /// The text that is displayed for this dialogue topic.
        /// </summary>
        [SerializeField]
        private string displayText;

        /// <summary>
        /// The method used to select the response from the response list.
        /// </summary>
        [SerializeField]
        private EResponseSelectMethod responseSelectMethod;

        /// <summary>
        /// The responses that belong to this dialogue option.
        /// </summary>
        [SerializeField]
        private DialogueResponseData[] responses;

        /// <summary>
        /// Flag indicating if conversations should cease after this topic is selected.
        /// </summary>
        [SerializeField]
        private bool isExitTrigger;

        /// <summary>
        /// Events that could occur as a result of this topic (eg. NPC gives player item).
        /// </summary>
        [SerializeField]
        private GameEventData[] gameEventData;

        /// <summary>
        /// Conditions needed for this topic to be used.
        /// </summary>
        [SerializeField]
        private ConditionData[] conditionData;

        /// <summary>
        /// Gets the text to display for this topic.
        /// </summary>
        public string DisplayText {
            get { return displayText; }
        }

        /// <summary>
        /// Gets method used for response selection.
        /// </summary>
        public EResponseSelectMethod ResponseSelectMethod {
            get { return responseSelectMethod; }
        }

        /// <summary>
        /// Gets the collection of responses added to this topic.
        /// </summary>
        public DialogueResponseData[] DialogueResponses {
            get { return responses; }
        }

        /// <summary>
        /// Gets the flag indicating if the topic should cause dialogue to exit.
        /// </summary>
        public bool IsExitTrigger {
            get { return isExitTrigger; }
        }

        /// <summary>
        /// Gets the game events attached to this topic.
        /// </summary>
        public GameEventData[] GameEventData {
            get { return gameEventData; }
        }

        /// <summary>
        /// Gets the conditions required for this topic.
        /// </summary>
        public ConditionData[] ConditionData {
            get { return conditionData; }
        }
    }
}
