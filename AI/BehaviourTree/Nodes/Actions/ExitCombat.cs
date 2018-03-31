using JusticeFramework.Components;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class ExitCombat : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			self.ExitCombat();
			return ENodeStatus.Success;
		}
	}
}