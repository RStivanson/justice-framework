using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.Data.AI.BehaviourTree.Builder {
	public class LeafBuilder<TParent, TNode> where TNode : Leaf {
		private readonly TParent parent;
		private TNode node;

		public LeafBuilder(TParent parent, TNode node) {
			this.parent = parent;
			this.node = node;
		}
		
		public TParent End() {
			return parent;
		}
	}
}