using JusticeFramework.Core.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Models.Dialogue {
	[Serializable]
	public class Response {
		/// <summary>
		/// The text used when displaying a response back to the player's choice
		/// </summary>
		[SerializeField]
		private string responseText;
		
		/// <summary>
		/// The clip played while the response text is being shown
		/// </summary>
		[SerializeField]
		private AudioClip voiceover;
		
		/// <summary>
		/// The topic index that the branch should go to after this
		/// </summary>
		[SerializeField]
		private int linkToIndex;
		
		/// <summary>
		/// Flag indicating if conversations should cease after this response is played
		/// </summary>
		[SerializeField]
		private bool isExitTrigger;
		
		/// <summary>
		/// Flag indicating if the parent branch should reset after this response is played
		/// </summary>
		[SerializeField]
		private bool shouldResetBranch;
		
		/// <summary>
		/// Events that could occur as a result of this response (eg. NPC gives player item)
		/// </summary>
		[SerializeField]
		private List<GameEvent> dialogueEvents;

        /// <summary>
        /// Conditions needed for this response to be used
        /// </summary>
        [SerializeField]
        private List<Condition> conditions;

        /// <summary>
        /// Gets or sets the text displayed to the player
        /// </summary>
		public string ResponseText {
			get { return responseText; }
			set { responseText = value; }
		}

        /// <summary>
        /// Gets or sets the narration clip
        /// </summary>
		public AudioClip Voiceover {
			get { return voiceover; }
			set { voiceover = value; }
		}

        /// <summary>
        /// Gets or sets the link to the next topic in the conversation
        /// </summary>
		public int TopicIndexLink {
			get { return linkToIndex; }
			set { linkToIndex = value; }
		}

        /// <summary>
        /// Gets or sets the flag indicating if the response should cause dialogue to exit
        /// </summary>
		public bool IsExitTrigger {
			get { return isExitTrigger; }
			set { isExitTrigger = value; }
		}

        /// <summary>
        /// Flag indicating if the response should cause the branch to reset
        /// </summary>
		public bool ShouldResetBranch {
			get { return shouldResetBranch; }
			set { shouldResetBranch = value; }
		}

        /// <summary>
        /// Processes all events on the given target
        /// </summary>
        /// <param name="target">The target for the dialogue events</param>
        public void ProcessEvents(IActor self, IActor target) {
            // If the target is null, do nothing
            if (target == null || dialogueEvents == null) {
                return;
            }

            // Process each event on the target
            foreach (GameEvent dialogueEvent in dialogueEvents) {
                dialogueEvent?.Execute(self, target);
            }
		}

        /// <summary>
        /// Determines if the conditions for this response are met
        /// </summary>
        /// <param name="target">The target to check against</param>
        /// <returns>Return true if the conditions are met or empty, false otherwise</returns>
        public bool MeetsConditions(IEntity self, IEntity target) {
            bool result = true;

            // If the target is not null and the conditions arent null
            if (target != null && conditions != null) {
                // Evaluate each condition
                foreach (Condition condition in conditions) {
                    result &= condition.Evaluate(self, target);
                }
            }

            return result;
        }
	}
}