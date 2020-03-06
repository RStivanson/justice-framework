using System;
using System.Collections.Generic;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	[Serializable]
	public class MemSequence : Composite {
		public MemSequence() {
		}
		
		public MemSequence(List<Node> tasks) : base(tasks) {
		}
		
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			int startIndex = (int)tick.blackboard.Get("runningChild", Id);
			
			for (int i = startIndex; i < tasks.Count; i++) {
				ENodeStatus childStatus = tasks[i].Tick(tick);

				if (childStatus != ENodeStatus.Success) {
					if (childStatus == ENodeStatus.Running) {
						tick.blackboard.Set("runningChild", i, Id);
					}
					
					return childStatus;
				}
			}

			return ENodeStatus.Success;
		}

		protected override void Open(BehaviourTree.Context tick) {
			tick.blackboard.Set("runningChild", 0, Id);
		}
	}
}