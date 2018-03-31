using JusticeFramework.Data.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourTree.Definitions {
	public class Combat : BehaviourSet {
		protected override Data.AI.BehaviourTree.BehaviourTree BuildBehaviourSet() {
			return null;/*new BehaviourTreeBuilder()
				.Composite<Selector>()
					.Composite<Sequence>()
						.Leaf<IsInCombat>().End()
						.Composite<Selector>()
							.Composite<Sequence>()
								.Leaf<HasTarget>().End()
								.Composite<Selector>()
									.Leaf<TargetWithinInterestDistance>().End()
									.Decorator<AlwaysFail>()
										.Leaf<ClearTarget>().End()
									.End()
								.End()
								.Leaf<UpdatePathToTarget>().End()
							.End()
							.Composite<Sequence>()
								.Leaf<GetNextAttacker>().End()
								.Leaf<UpdatePathToTarget>().End()
							.End()
						.End()
						.Composite<MemSelector>()
							.Composite<MemSequence>()
								.Leaf<ShouldRunAway>().End()
								.Leaf<SetLocationRandom>().End()
								.Leaf<IsDoneMovingToTarget>().End()
							.End()
							.Composite<MemSequence>()
								.Leaf<InRangeForMeleeAttack>(5).End()
								.Leaf<StopMoving>().End()
								.Leaf<LookAtTarget>().End()
								.Leaf<Attack>().End()
							.End()
							.Leaf<IsDoneMovingToTarget>().End()
						.End()
					.End()
					.Composite<Sequence>()
						.Leaf<TargetNearbyEnemy>().End()
						.Leaf<UpdatePathToTarget>().End()
					.End()
				.End()
			.Build();*/
		}
	}
}