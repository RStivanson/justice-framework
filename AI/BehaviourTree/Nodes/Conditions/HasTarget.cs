using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Conditions {
	public class HasTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			return (tick.blackboard.Get("target") != null) ? ENodeStatus.Success : ENodeStatus.Failure;
		}
	}
}