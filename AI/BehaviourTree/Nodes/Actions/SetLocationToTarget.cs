using JusticeFramework.Components;
using JusticeFramework.Data.AI.BehaviourTree;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;
using UnityEngine;
using UnityEngine.AI;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class SetLocationToTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Reference target = tick.blackboard.Get<Reference>("target");

			if (target == null) {
				return ENodeStatus.Failure;
			}

			controller.Agent.SetDestination(target.Transform.position);
			return ENodeStatus.Success;
		}
	}
}