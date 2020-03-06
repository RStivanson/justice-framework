using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class IsDead : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			
			return controller.Actor.IsDead ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}