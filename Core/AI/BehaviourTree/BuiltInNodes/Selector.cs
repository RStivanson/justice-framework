using System;
using System.Collections.Generic;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	[Serializable]
	public class Selector : Composite {
		public Selector() {
		}
		
		public Selector(List<Node> tasks) : base(tasks) {
		}
		
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			for (int i = 0; i < tasks.Count; i++) {
				ENodeStatus childStatus = tasks[i].Tick(tick);

				if (childStatus != ENodeStatus.Failure) {
					return childStatus;
				}
			}
			
			return ENodeStatus.Failure;
		}
	}
}