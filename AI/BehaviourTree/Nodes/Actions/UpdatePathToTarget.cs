using JusticeFramework.Components;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class UpdatePathToTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Reference target = tick.blackboard.Get<Reference>("target");

			controller.Agent.SetDestination(target.Transform.position);

			return ENodeStatus.Success;
		}
	}
}