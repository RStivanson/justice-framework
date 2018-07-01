using System;
using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes.Decorators {
	[Serializable]
	public class Loop : Decorator {
		[SerializeField]
		[HideInInspector]
		private int iterationLimit;

		[SerializeField]
		[HideInInspector]
		private int iterationCount;
		
		public Loop(int loopLimit, Node task = null) : base(task) {
			iterationLimit = loopLimit;
		}

		protected override void Open(TickState tick) {
			iterationCount = 0;
		}

		protected override ENodeStatus OnTick(TickState tick) {
			if (task == null) {
				return ENodeStatus.Error;
			}

			if (iterationCount >= iterationLimit) {
				return ENodeStatus.Success;
			}

			iterationCount++;
			task.Tick(tick);

			return ENodeStatus.Running;
		}
	}
}