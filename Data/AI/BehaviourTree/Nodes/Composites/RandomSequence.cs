using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace JusticeFramework.Data.AI.BehaviourTree.Nodes.Composites {
	[Serializable]
	public class RandomSequence : Composite {
		public RandomSequence() {
		}
		
		public RandomSequence(List<Node> tasks) : base(tasks) {
		}
		
		protected override ENodeStatus OnTick(TickState tick) {
			foreach (Node task in tasks) {
				int randomIndex = (int)(Random.value % tasks.Count);
				
				ENodeStatus childStatus = tasks[randomIndex].Tick(tick);

				if (childStatus != ENodeStatus.Success) {
					return childStatus;
				}
			}
			
			return ENodeStatus.Success;
		}
	}
}