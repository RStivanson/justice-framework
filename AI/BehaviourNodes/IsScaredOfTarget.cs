using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class IsScaredOfTarget : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			object target = tick.blackboard.Get("target");

			if (target is Actor) {
				Actor actor = (Actor)target;
				return controller.Actor.IsScaredOf(actor) ? ENodeStatus.Success : ENodeStatus.Failure;
			}
			
			return ENodeStatus.Failure;
		}
	}
}