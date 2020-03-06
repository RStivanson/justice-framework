using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class IsInCombat : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");

			return controller.Actor.IsInCombat ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}