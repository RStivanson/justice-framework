using System;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	[Serializable]
	public class UntilSucceed : Decorator {
		public UntilSucceed() {
		}
		
		public UntilSucceed(Node task) : base(task) {
		}

		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
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