using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class Attack : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");

			if (!controller.Actor.IsRightSwinging()) {
				controller.Actor.Swing();
			}

			return ENodeStatus.Success;
		}
	}
}