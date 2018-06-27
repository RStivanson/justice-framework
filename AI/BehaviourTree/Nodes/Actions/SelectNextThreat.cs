using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;
using UnityEngine.AI;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class SelectNextThreat : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
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
			
			return ENodeStatus.Success;
		}
	}
}