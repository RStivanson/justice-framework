using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes {
	[Serializable]
	public abstract class Composite : Node {
		[SerializeField]
		[HideInInspector]
		protected List<Node> tasks;

		public Composite() : this(null) {
		}
		
		public Composite(List<Node> tasks) {
			this.tasks = tasks;
		}

		public void AddChild(Node node) {
			if (tasks == null) {
				tasks = new List<Node>();
			}
			
			tasks.Add(node);
		}
	}
}