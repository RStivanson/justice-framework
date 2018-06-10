using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;
using UnityEngine;
using UnityEngine.AI;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class SetLocationRandom : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Vector3 location;
			
			while (!RandomPoint(controller.Actor.Transform.position, 20f, out location));
			
			tick.blackboard.Set("targetLocation", location);
			controller.Agent.SetDestination(location);

			return ENodeStatus.Success;
		}
		
		private static bool RandomPoint(Vector3 center, float range, out Vector3 result) {
			result = Vector3.zero;
			
			for (int i = 0; i < 30; i++) {
				Vector3 randomPoint = center + Random.insideUnitSphere * range;
				NavMeshHit hit;
				
				if (!NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
					continue;
				}

				result = hit.position;
				return true;
			}
			
			return false;
		}
	}
}