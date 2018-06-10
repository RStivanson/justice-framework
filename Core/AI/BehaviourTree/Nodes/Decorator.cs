using System;
using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes {
	[Serializable]
	public abstract class Decorator : Node {
		[SerializeField]
		[HideInInspector]
		protected Node task;

		public Decorator() : this(null) {
		}
		
		public Decorator(Node task) {
			this.task = task;
		}

		public void SetChild(Node task) {
			this.task = task;
		}
	}
}