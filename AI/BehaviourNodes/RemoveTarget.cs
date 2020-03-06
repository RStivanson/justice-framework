using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class RemoveTarget : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			Actor target = tick.blackboard.Get<Actor>("target");

			self.RemoveThreat(target);
			tick.blackboard.Set("target", null);
			
			return ENodeStatus.Success;
		}
	}
}