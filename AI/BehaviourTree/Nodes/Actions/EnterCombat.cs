using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class EnterCombat : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			self.EnterCombat(tick.blackboard.Get<Actor>("target"));
			return ENodeStatus.Success;
		}
	}
}