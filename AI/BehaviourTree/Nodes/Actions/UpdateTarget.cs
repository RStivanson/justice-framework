using JusticeFramework.Components;
using JusticeFramework.Data;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;
using UnityEngine;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class UpdateTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Actor self = controller.Actor;

			if (self.Threats.Count > 0) {
				tick.blackboard.Set("target", self.Threats[self.Threats.Count - 1]);
				return ENodeStatus.Success;
			}

			if (self.Aggression != EAggression.Passive) {
				Actor[] nearbyActors = controller.Vision.NearbyQuery<Actor>(x => !x.IsDead, self.LoseInterestDistance);

				if (nearbyActors != null) {
					foreach (Actor actor in nearbyActors) {
						// TODO: Check if the actor is actually an enemy before assigning it as a target
						if (self.IsScared(actor)) {
							continue;
						}

						tick.blackboard.Set("target", actor);
						self.EnterCombat(actor);
						return ENodeStatus.Success;
					}
				}
			}
			
			tick.blackboard.Set("target", null);
			return ENodeStatus.Failure;
		}
	}
}