using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class UpdatePathToTarget : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			WorldObject target = tick.blackboard.Get<WorldObject>("target");

			controller.Agent.SetDestination(target.Transform.position);

			return ENodeStatus.Success;
		}
	}
}