using JusticeFramework.Components;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Conditions {
	public class TargetInRange : Leaf {
		private float targetRange;

		public TargetInRange(float range) {
			targetRange = range;
		}
		
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Reference target = tick.blackboard.Get<Reference>("target");

			if (target == null || (controller.Actor.Transform.position - target.Transform.position).sqrMagnitude > targetRange) {
				return ENodeStatus.Failure;
			}

			return ENodeStatus.Success;
		}
	}
}