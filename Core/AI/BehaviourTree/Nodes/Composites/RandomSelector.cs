using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes.Composites {
	[Serializable]
	public class RandomSelector : Composite {
		public RandomSelector() {
		}
		
		public RandomSelector(List<Node> tasks) : base(tasks) {
		}
		
		protected override ENodeStatus OnTick(TickState tick) {
			foreach (Node task in tasks) {
				int randomIndex = (int)(Random.value % tasks.Count);
				
				ENodeStatus childStatus = tasks[randomIndex].Tick(tick);

				if (childStatus != ENodeStatus.Failure) {
					return childStatus;
				}
			}
			
			return ENodeStatus.Failure;
		}
	}
}