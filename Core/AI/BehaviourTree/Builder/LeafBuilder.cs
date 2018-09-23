using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.Core.AI.BehaviourTree.Builder {
	public class LeafBuilder<TParent, TNode> where TNode : Leaf {
		private readonly TParent parent;
		private TNode node;

        public TNode Node {
            get { return Node; }
        }

		public LeafBuilder(TParent parent, TNode node) {
			this.parent = parent;
			this.node = node;
		}
		
		public TParent End() {
			return parent;
		}
	}
}