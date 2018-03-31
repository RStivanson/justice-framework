using JusticeFramework.AI.BehaviourTree.Nodes.Actions;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Builder;
using JusticeFramework.Data.AI.BehaviourTree.Nodes.Composites;
using JusticeFramework.Data.AI.BehaviourTree.Nodes.Leafs;

namespace JusticeFramework.AI.BehaviourTree.Definitions {
	public class Wander : BehaviourSet {
		protected override Data.AI.BehaviourTree.BehaviourTree BuildBehaviourSet() {
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