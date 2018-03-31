using JusticeFramework.AI.BehaviourTree.Nodes.Actions;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Builder;
using JusticeFramework.Data.AI.BehaviourTree.Nodes.Composites;

namespace JusticeFramework.AI.BehaviourTree.Definitions {
	public class MoveTo : BehaviourSet {
		protected override Data.AI.BehaviourTree.BehaviourTree BuildBehaviourSet() {
			return new BehaviourTreeBuilder()
					.Composite<MemSequence>()
						.Leaf<SetLocation>().End()
						.Leaf<MoveToDestination>().End()
					.End()
				.Build();
		}
	}
}