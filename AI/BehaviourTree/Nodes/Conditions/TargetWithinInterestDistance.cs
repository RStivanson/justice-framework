using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Conditions {
	public class TargetWithinInterestDistance : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			WorldObject target = tick.blackboard.Get<WorldObject>("target");

			return self.InInterestDistance(target) ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}