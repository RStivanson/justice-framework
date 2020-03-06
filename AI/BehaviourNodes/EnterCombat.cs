using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class EnterCombat : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			self.EnterCombat(tick.blackboard.Get<Actor>("target"));
			return ENodeStatus.Success;
		}
	}
}