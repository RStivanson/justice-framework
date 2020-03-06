using System;
using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	[Serializable]
	public class Limit : Decorator {
		[SerializeField]
		[HideInInspector]
		private int tickLimit;
		
		public Limit(int tickLimit) : this(tickLimit, null) {
		}
		
		public Limit(int tickLimit, Node task) : base(task) {
			this.tickLimit = tickLimit;
		}

		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			if (task == null) {
				return ENodeStatus.Error;
			}

			if (task.TickCount >= tickLimit) {
				return ENodeStatus.Failure;
			}
			
			return task.Tick(tick);
		}
	}
}