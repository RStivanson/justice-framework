using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class ExitCombat : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			self.ExitCombat();
			return ENodeStatus.Success;
		}
	}
}