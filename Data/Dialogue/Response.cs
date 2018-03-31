using System;
using UnityEngine;

namespace JusticeFramework.Data.Dialogue {
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
		private DialogueEvent dialogueEvent;

		public string ResponseText {
			get { return responseText; }
			set { responseText = value; }
		}

		public AudioClip Voiceover {
			get { return voiceover; }
			set { voiceover = value; }
		}

		public int TopicIndexLink {
			get { return linkToIndex; }
			set { linkToIndex = value; }
		}

		public bool IsExitTrigger {
			get { return isExitTrigger; }
			set { isExitTrigger = value; }
		}

		public bool ShouldResetBranch {
			get { return shouldResetBranch; }
			set { shouldResetBranch = value; }
		}
		
		public void ProcessEvents(object target) {
			dialogueEvent?.Execute(target);
		}
	}
}