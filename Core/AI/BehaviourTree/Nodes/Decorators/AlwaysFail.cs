using System;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes.Decorators {
	[Serializable]
	public class AlwaysFail : Decorator {
		public AlwaysFail() {
		}
		
		public AlwaysFail(Node task) : base(task) {
		}

		protected override ENodeStatus OnTick(TickState tick) {
			if (task == null) {
				return ENodeStatus.Error;
			}
			
			ENodeStatus childStatus = task.Tick(tick);

			if (childStatus == ENodeStatus.Running) {
				return childStatus;
			}
			
			return ENodeStatus.Failure;
		}
	}
}