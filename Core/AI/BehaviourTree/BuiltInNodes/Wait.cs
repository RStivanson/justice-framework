using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	public class Wait : Leaf {
		private readonly float waitLength;

		public Wait(float length) {
			waitLength = length;
		}
		
		protected override void Open(BehaviourTree.Context tick) {
			tick.blackboard.Set("startTime", Time.time, Id);
		}

		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			float startTime = (float)tick.blackboard.Get("startTime", Id);

			if ((Time.time - startTime) >= waitLength) {
				return ENodeStatus.Success;
			}

			return ENodeStatus.Running;
		}
	}
}