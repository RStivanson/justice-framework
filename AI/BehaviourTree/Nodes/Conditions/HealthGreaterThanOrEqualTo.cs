using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Conditions {
	public class HealthGreaterThanOrEqualTo : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			float compareValue = tick.blackboard.Get<float>("hgte_healthTarget");
			
			return controller.Actor.CurrentHealth >= compareValue ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}