using System;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	[Serializable]
	public class AlwaysSucceed : Decorator {
		public AlwaysSucceed() {
		}
		
		public AlwaysSucceed(Node task) : base(task) {
		}

		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			if (task == null) {
				return ENodeStatus.Error;
			}
			
			ENodeStatus childStatus = task.Tick(tick);

			if (childStatus == ENodeStatus.Running) {
				return childStatus;
			}
			
			return ENodeStatus.Success;
		}
	}
}