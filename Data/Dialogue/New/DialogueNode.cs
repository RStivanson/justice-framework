using System;
using JusticeFramework.Data.DataStructures.Graph;
using JusticeFramework.Data.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data.Dialogue {
	[Serializable]
	public class DialogueNode : Node {
		/// <summary>
		/// The text used when displaying a response back to the player's choice
		/// </summary>
		[SerializeField]
		private string dialogue;
		
		/// <summary>
		/// The clip played while the response text is being shown
		/// </summary>
		[SerializeField]
		private AudioClip voiceover;
		
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

		[SerializeField]
		private EDialogueType dialogueType;
		
		public string Dialogue {
			get { return dialogue; }
			set { dialogue = value; }
		}

		public AudioClip Voiceover {
			get { return voiceover; }
			set { voiceover = value; }
		}

		public bool IsExitTrigger {
			get { return isExitTrigger; }
			set { isExitTrigger = value; }
		}

		public bool ShouldResetBranch {
			get { return shouldResetBranch; }
			set { shouldResetBranch = value; }
		}

		public DialogueEvent DialogueEvent {
			get { return dialogueEvent; }
			set { dialogueEvent = value; }
		}
		
		public EDialogueType DialogueType {
			get { return dialogueType; }
			set { dialogueType = value; }
		}
		
		public DialogueNode() : base() {	
		}
		
		public DialogueNode(int id) : base(id) {
		}

		public DialogueNode(EDialogueType dialogueType, string dialogue, AudioClip voiceover, bool isExitTrigger, bool shouldResetBranch) {
			this.dialogueType = dialogueType;
			this.dialogue = dialogue;
			this.voiceover = voiceover;
			this.isExitTrigger = isExitTrigger;
			this.shouldResetBranch = shouldResetBranch;
		}
		
		public void ProcessEvents(IWorldObject target) {
			dialogueEvent?.Execute(target);
		}
	}
}