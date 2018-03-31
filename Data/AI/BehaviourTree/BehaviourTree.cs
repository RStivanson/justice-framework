using JusticeFramework.Data.AI.BehaviourTree.Nodes;
using JusticeFramework.Data.Interfaces;

namespace JusticeFramework.Data.AI.BehaviourTree {
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