using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class HealthGreaterThan : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			float compareValue = tick.blackboard.Get<float>("hgt_healthTarget");
			
			return controller.Actor.CurrentHealth > compareValue ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}