using System;
using JusticeFramework.Data.DataStructures.Graph;
using JusticeFramework.Data.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data.Dialogue {
	[Serializable]
	public class DialogueTree : Graph<DialogueNode> {
		[SerializeField]
		private int headNode;
		
		[SerializeField]
		private int lastActiveResponse;

		public int AddNode(EDialogueType type, string dialogue, bool isExitNode, bool shouldResetHead, bool makeHead = false) {
			int index = AddNode(new DialogueNode(type, dialogue, null, isExitNode, shouldResetHead));

			if (makeHead) {
				headNode = index;
			}
			
			return index;
		}

		public void Reset() {
			lastActiveResponse = headNode;
		}
		
		private void Activate(DialogueNode node, IWorldObject target) {
			lastActiveResponse = node.Id;
			
			node.ProcessEvents(target);
		}
	}
}