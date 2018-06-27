using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class Attack : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");

			if (!controller.Actor.IsRightSwinging()) {
				//controller.Actor.Swing();
			}

			return ENodeStatus.Success;
		}
	}
}