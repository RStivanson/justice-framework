using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.Nodes.Leafs {
	public class Print : Leaf {
		private readonly string message;

		public Print(string message) {
			this.message = message;
		}

		protected override ENodeStatus OnTick(TickState tick) {
			Debug.Log(message);
			return ENodeStatus.Success;
		}
	}
}