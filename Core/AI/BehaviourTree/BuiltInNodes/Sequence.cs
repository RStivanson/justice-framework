using System;
using System.Collections.Generic;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	[Serializable]
	public class Sequence : Composite {
		public Sequence() {
		}
		
		public Sequence(List<Node> tasks) : base(tasks) {
		}
		
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			for (int i = 0; i < tasks.Count; i++) {
				ENodeStatus childStatus = tasks[i].Tick(tick);

				if (childStatus != ENodeStatus.Success) {
					return childStatus;
				}
			}

			return ENodeStatus.Success;
		}
	}
}