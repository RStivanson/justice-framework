using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class HasTarget : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			return (tick.blackboard.Get("target") != null) ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}