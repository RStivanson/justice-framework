using System;
using System.Collections.Generic;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes.Composites {
	[Serializable]
	public class Parallel : Composite {
		public enum ERequirement {
			One,
			All
		}

		private readonly ERequirement success;
		private readonly ERequirement fail;

		public Parallel(ERequirement success, ERequirement fail, List<Node> tasks = null) : base(tasks) {
			this.success = success;
			this.fail = fail;
		}
		
		protected override ENodeStatus OnTick(TickState tick) {
			int successCount = 0;
			int failureCount = 0;

			foreach (Node task in tasks) {
				switch (task.Tick(tick)) {
					case ENodeStatus.Success:
						successCount++;

						if (success == ERequirement.One) {
							return ENodeStatus.Success;
						}

						break;
					case ENodeStatus.Failure:
						failureCount++;

						if (fail == ERequirement.One) {
							return ENodeStatus.Failure;
						}

						break;
				}
			}

			if (fail == ERequirement.All && failureCount == tasks.Count) {
				return ENodeStatus.Failure;
			}
			
			if (success == ERequirement.All && successCount == tasks.Count) {
				return ENodeStatus.Success;
			}
			
			return ENodeStatus.Running;
		}
	}
}