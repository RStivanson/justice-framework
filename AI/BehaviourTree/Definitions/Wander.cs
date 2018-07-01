using JusticeFramework.AI.BehaviourTree.Nodes.Actions;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Builder;
using JusticeFramework.Core.AI.BehaviourTree.Nodes.Composites;
using JusticeFramework.Core.AI.BehaviourTree.Nodes.Leafs;

namespace JusticeFramework.AI.BehaviourTree.Definitions {
	public class Wander : BehaviourSet {
		protected override Core.AI.BehaviourTree.BehaviourTree BuildBehaviourSet() {
			return new BehaviourTreeBuilder()
					.Composite<MemSequence>()
						.Leaf<SetLocationRandom>().End()
						.Leaf<MoveToDestination>().End()
						.Leaf<Wait>(5).End()
					.End()
				.Build();
		}
	}
}