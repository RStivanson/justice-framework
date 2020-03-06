namespace JusticeFramework.Core.AI.BehaviourTree {
    public class BehaviourTree {
		private static int nodeIdCounter = 0;

        public struct Context {
            public Blackboard blackboard;
            public bool debug;

            public Context(Blackboard blackboard, bool debug) {
                this.blackboard = blackboard;
                this.debug = debug;
            }
        }

        private int id;
		private Node root;
		
		public int Id {
			get { return id; }
		}

		public BehaviourTree(Node rootNode) {
			id = nodeIdCounter++;
			root = rootNode;
		}
		
		public void Tick(Context tick) {
			root.Tick(tick);
		}
	}
}