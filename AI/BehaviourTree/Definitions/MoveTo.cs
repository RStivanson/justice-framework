using JusticeFramework.AI.BehaviourTree.Nodes.Actions;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Builder;
using JusticeFramework.Core.AI.BehaviourTree.Nodes.Composites;

namespace JusticeFramework.AI.BehaviourTree.Definitions {
	public class MoveTo : BehaviourSet {
		protected override Core.AI.BehaviourTree.BehaviourTree BuildBehaviourSet() {
			return new BehaviourTreeBuilder()
					.Composite<MemSequence>()
						.Leaf<SetLocation>().End()
						.Leaf<MoveToDestination>().End()
					.End()
				.Build();
		}
	}
}