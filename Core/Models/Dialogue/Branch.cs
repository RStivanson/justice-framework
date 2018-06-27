using System;
using UnityEngine;

namespace JusticeFramework.Core.Models.Dialogue {
	[Serializable]
	public class Branch {
		[SerializeField]
		private readonly int id;
		
		[SerializeField]
		private int headTopic;
		
		[SerializeField]
		private int currentTopic;

		public int Id {
			get { return id; }
		}

		public int HeadTopic {
			get { return headTopic; }
			set { headTopic = value; }
		}
		
		public int CurrentTopic {
			get { return currentTopic; }
			set { currentTopic = value; }
		}

		public Branch(int id) {
			this.id = id;
			headTopic = 0;
			currentTopic = 0;
		}
		
		public void Reset() {
			currentTopic = headTopic;
		}
		
		public void Advance(Response response) {
			if (response.ShouldResetBranch || response.TopicIndexLink < 0) {
				Reset();
			} else {
				currentTopic = response.TopicIndexLink;
			}
		}
	}
}