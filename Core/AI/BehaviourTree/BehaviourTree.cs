using JusticeFramework.Core.AI.BehaviourTree.Nodes;
using JusticeFramework.Core.Interfaces;

namespace JusticeFramework.Core.AI.BehaviourTree {
	public class BehaviourTree {
		private static int nodeIdCounter = 0;

		private int id;
		private Node root;
		
		public int Id {
			get { return id; }
		}

		public BehaviourTree(Node rootNode) {
			id = nodeIdCounter++;
			root = rootNode;
		}
		
		public void Tick(TickState tick) {
			root.Tick(tick);
		}
	}
}