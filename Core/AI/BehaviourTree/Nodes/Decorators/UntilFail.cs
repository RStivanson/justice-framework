using System;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes.Decorators {
	[Serializable]
	public class UntilFail : Decorator {
		public UntilFail() {
		}
		
		public UntilFail(Node task) : base(task) {
		}

		protected override ENodeStatus OnTick(TickState tick) {
			if (task == null) {
				return ENodeStatus.Error;
			}

			if (task.Tick(tick) != ENodeStatus.Failure) {
				return ENodeStatus.Running;
			}
			
			return ENodeStatus.Success;
		}
	}
}