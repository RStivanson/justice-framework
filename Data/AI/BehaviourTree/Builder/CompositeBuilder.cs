using System;
using JusticeFramework.Data.AI.BehaviourTree.Nodes;

namespace JusticeFramework.Data.AI.BehaviourTree.Builder {
	public class CompositeBuilder<TParent, TNode> where TNode : Composite {
		private readonly TParent parent;
		private TNode node;

		public CompositeBuilder(TParent parent, TNode node) {
			this.parent = parent;
			this.node = node;
		}
		
		public CompositeBuilder<CompositeBuilder<TParent, TNode>, Composite> Composite<T>(params object[] arguments) where T : Composite {
			Composite composite = (T)Activator.CreateInstance(typeof(T), arguments);
			node.AddChild(composite);
			
			return new CompositeBuilder<CompositeBuilder<TParent, TNode>, Composite>(this, composite);
		}
		
		public DecoratorBuilder<CompositeBuilder<TParent, TNode>, Decorator> Decorator<T>(params object[] arguments) where T : Decorator {
			Decorator decorator = (T)Activator.CreateInstance(typeof(T), arguments);
			node.AddChild(decorator);
 
			return new DecoratorBuilder<CompositeBuilder<TParent, TNode>, Decorator>(this, decorator);
		}
		
		public LeafBuilder<CompositeBuilder<TParent, TNode>, Leaf> Leaf<T>(params object[] arguments) where T : Leaf {
			Leaf leaf = (T)Activator.CreateInstance(typeof(T), arguments);
			node.AddChild(leaf);
			
			return new LeafBuilder<CompositeBuilder<TParent, TNode>, Leaf>(this, leaf);
		}

		public CompositeBuilder<TParent, TNode> Include(Node includeNode) {
			node.AddChild(includeNode);
			return this;
		}
		
		public TParent End() {
			return parent;
		}
	}
}