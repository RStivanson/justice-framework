using System;
using UnityEngine;

namespace JusticeFramework.Data.Dialogue {
	[Serializable]
	public class Conversation : ScriptableObject {
		[SerializeField]
		public string id;

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