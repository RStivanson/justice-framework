using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class UpdatePathToTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			WorldObject target = tick.blackboard.Get<WorldObject>("target");

			controller.Agent.SetDestination(target.Transform.position);

			return ENodeStatus.Success;
		}
	}
}