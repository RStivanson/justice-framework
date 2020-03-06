using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class TargetWithinInterestDistance : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			WorldObject target = tick.blackboard.Get<WorldObject>("target");

			return self.InInterestDistance(target) ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}