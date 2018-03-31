using JusticeFramework.Components;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Conditions {
	public class TargetWithinInterestDistance : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			Reference target = tick.blackboard.Get<Reference>("target");

			return self.InInterestDistance(target) ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}