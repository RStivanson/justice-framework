using System;

namespace JusticeFramework.Data.AI.BehaviourTree.Nodes.Decorators {
	[Serializable]
	public class UntilSucceed : Decorator {
		public UntilSucceed() {
		}
		
		public UntilSucceed(Node task) : base(task) {
		}

		protected override ENodeStatus OnTick(TickState tick) {
			if (task == null) {
				return ENodeStatus.Error;
			}

			if (task.Tick(tick) != ENodeStatus.Success) {
				return ENodeStatus.Running;
			}
			
			return ENodeStatus.Success;
		}
	}
}