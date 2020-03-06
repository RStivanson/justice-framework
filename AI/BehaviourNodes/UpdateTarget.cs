using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Data;

namespace JusticeFramework.AI.BehaviourNodes {
    public class UpdateTarget : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Actor self = controller.Actor;
            ActorData data = self.GetData<ActorData>();

			if (self.Threats.Count > 0) {
				tick.blackboard.Set("target", self.Threats[self.Threats.Count - 1]);
				return ENodeStatus.Success;
			}

			if (data.AiData.Aggression != EAggression.Passive) {
				Actor[] nearbyActors = controller.Vision.NearbyQuery<Actor>(x => !x.IsDead, data.AiData.LoseInterestDistanceSqr);

				if (nearbyActors != null) {
					foreach (Actor actor in nearbyActors) {
						// TODO: Check if the actor is actually an enemy before assigning it as a target
						if (self.IsScaredOf(actor)) {
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