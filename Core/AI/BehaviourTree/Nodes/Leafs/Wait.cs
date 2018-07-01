using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes.Leafs {
	public class Wait : Leaf {
		private readonly float waitLength;

		public Wait(float length) {
			waitLength = length;
		}
		
		protected override void Open(TickState tick) {
			tick.blackboard.Set("startTime", Time.time, Id);
		}

		protected override ENodeStatus OnTick(TickState tick) {
			float startTime = (float)tick.blackboard.Get("startTime", Id);

			if ((Time.time - startTime) >= waitLength) {
				return ENodeStatus.Success;
			}

			return ENodeStatus.Running;
		}
	}
}