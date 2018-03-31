using JusticeFramework.Components;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class RemoveTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			Actor target = tick.blackboard.Get<Actor>("target");

			self.RemoveThreat(target);
			tick.blackboard.Set("target", null);
			
			return ENodeStatus.Success;
		}
	}
}