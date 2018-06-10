using System;
using UnityEngine;

namespace JusticeFramework.Core.Models.Dialogue {
	[Serializable]
	public class Conversation : EntityModel {
		[SerializeField]
		public bool isFactionDialogue;
		
		[SerializeField]
		public string targetId;

		[SerializeField]
		public Branch[] branches;
		
		[SerializeField]
		public Topic[] topics;

		public string Id {
			get { return id; }
			set { id = value; }
		}
		
		public Topic GetTopic(Branch branch) {
			return topics[branch.CurrentTopic];
		}
	}
}