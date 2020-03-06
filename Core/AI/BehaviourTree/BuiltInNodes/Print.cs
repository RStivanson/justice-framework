using UnityEngine;

namespace JusticeFramework.Core.AI.BehaviourTree.BuiltInNodes {
	public class Print : Leaf {
		private readonly string message;

		public Print(string message) {
			this.message = message;
		}

		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			Debug.Log(message);
			return ENodeStatus.Success;
		}
	}
}