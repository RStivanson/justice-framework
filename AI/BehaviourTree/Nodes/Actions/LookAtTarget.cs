using JusticeFramework.Components;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;
using UnityEngine;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class LookAtTarget : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
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