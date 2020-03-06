using System;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Defines a NPC dialogue option said by an NPC.
    /// </summary>
    [Serializable]
    public class DialogueResponseData {
        /// <summary>
        /// The text used when displaying this response.
        /// </summary>
        [SerializeField]
        private string displayText;

        /// <summary>
        /// The clip played while the display text is being shown.
        /// </summary>
        [SerializeField]
        private AudioClip voiceoverClip;

        /// <summary>
        /// The topic index that the branch should go to after this.
        /// </summary>
        [SerializeField]
        private int linkToTopicIndex;

        /// <summary>
        /// Flag indicating if conversations should cease after this response is played.
        /// </summary>
        [SerializeField]
        private bool isExitTrigger;

        /// <summary>
        /// Events that could occur as a result of this response (eg. NPC gives player item).
        /// </summary>
        [SerializeField]
        private GameEventData[] gameEventData;

        /// <summary>
        /// The conditions needed for this response to be usable.
        /// </summary>
        [SerializeField]
        private ConditionData[] conditionData;

        /// <summary>
        /// Gets the display text.
        /// </summary>
		public string DisplayText {
            get { return displayText; }
        }

        /// <summary>
        /// Gets the voiceover narration clip.
        /// </summary>
        public AudioClip VoiceoverClip {
            get { return voiceoverClip; }
        }

        /// <summary>
        /// Gets the link to the next topic in the conversation.
        /// </summary>
		public int LinkToTopicIndex {
            get { return linkToTopicIndex; }
        }

        /// <summary>
        /// Gets the flag indicating if the response should cause dialogue to exit.
        /// </summary>
		public bool IsExitTrigger {
            get { return isExitTrigger; }
        }

        /// <summary>
        /// Gets the game events attached to this response.
        /// </summary>
        public GameEventData[] GameEventData {
            get { return gameEventData; }
        }

        /// <summary>
        /// Gets the conditions required for this response.
        /// </summary>
        public ConditionData[] ConditionData {
            get { return conditionData; }
        }
    }
}
