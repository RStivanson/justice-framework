using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Conditions {
	public class IsScaredOfTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			object target = tick.blackboard.Get("target");

			if (target is Actor) {
				Actor actor = (Actor)target;
				return controller.Actor.IsScared(actor) ? ENodeStatus.Success : ENodeStatus.Failure;
			}
			
			return ENodeStatus.Failure;
		}
	}
}