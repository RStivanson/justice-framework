using JusticeFramework.Core.AI.BehaviourTree;
using UnityEngine;

namespace JusticeFramework.AI.BehaviourNodes {
    public class SetLocation : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
			Vector3 newLocation = tick.blackboard.Get<Vector3>("targetLocation");
			Debug.Log("NL: " + newLocation);

			controller.Agent.SetDestination(newLocation);

			return ENodeStatus.Success;
		}
	}
}