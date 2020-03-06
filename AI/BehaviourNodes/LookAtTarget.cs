using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using UnityEngine;

namespace JusticeFramework.AI.BehaviourNodes {
    public class LookAtTarget : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			Actor self = tick.blackboard.Get<AiController>("controller").Actor;
			WorldObject target = tick.blackboard.Get<WorldObject>("target");

			Vector3 targetDirection = (target.Transform.position - self.Transform.position);
			float step = 5 * Time.deltaTime;
			
			Vector3 newDirection = Vector3.RotateTowards(self.Transform.forward, targetDirection, step, 0.0F);
			self.Transform.rotation = Quaternion.LookRotation(newDirection);

			return ENodeStatus.Success;
		}
	}
}