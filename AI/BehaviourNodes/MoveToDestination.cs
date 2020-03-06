using JusticeFramework.Core.AI.BehaviourTree;
using UnityEngine.AI;

namespace JusticeFramework.AI.BehaviourNodes {
    public class MoveToDestination : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");

			if (controller.Agent.pathStatus == NavMeshPathStatus.PathInvalid || controller.Agent.pathStatus == NavMeshPathStatus.PathInvalid) {
				return ENodeStatus.Failure;
			}
			
			if (controller.Agent.pathPending) {
				return ENodeStatus.Running;
			}

			if (controller.Agent.remainingDistance > controller.Agent.stoppingDistance) {
				return ENodeStatus.Running;
			}

            controller.Agent.ResetPath();
			
			return ENodeStatus.Success;
		}
	}
}