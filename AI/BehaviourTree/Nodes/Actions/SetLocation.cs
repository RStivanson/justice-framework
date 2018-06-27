using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;
using UnityEngine;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class SetLocation : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Vector3 newLocation = tick.blackboard.Get<Vector3>("targetLocation");
			Debug.Log("NL: " + newLocation);

			controller.Agent.SetDestination(newLocation);

			return ENodeStatus.Success;
		}
	}
}