using System;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	[Serializable]
	public class Not : Decorator {
		public Not() {
		}
		
		public Not(Node task) : base(task) {
		}

		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			if (task == null) {
				return ENodeStatus.Error;
			}
			
			ENodeStatus childStatus = task.Tick(tick);

			switch (childStatus) {
				case ENodeStatus.Success:
					return ENodeStatus.Failure;
				case ENodeStatus.Failure:
					return ENodeStatus.Success;
			}

			return childStatus;
		}
	}
}