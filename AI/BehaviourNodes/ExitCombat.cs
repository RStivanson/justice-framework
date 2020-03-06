using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class ExitCombat : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			self.ExitCombat();
			return ENodeStatus.Success;
		}
	}
}