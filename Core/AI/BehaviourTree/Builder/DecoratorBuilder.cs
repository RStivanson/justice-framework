using System;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.Core.AI.BehaviourTree.Builder {
	public class DecoratorBuilder<TParent, TNode> where TNode : Decorator {
		private readonly TParent parent;
		private TNode node;

		public DecoratorBuilder(TParent parent, TNode node) {
			this.parent = parent;
			this.node = node;
		}
		
		public CompositeBuilder<DecoratorBuilder<TParent, TNode>, Composite> Composite<T>(params object[] arguments) where T : Composite {
			Composite composite = (T)Activator.CreateInstance(typeof(T), arguments);
			node.SetChild(composite);
			
			return new CompositeBuilder<DecoratorBuilder<TParent, TNode>, Composite>(this, composite);
		}
		
		public DecoratorBuilder<DecoratorBuilder<TParent, TNode>, Decorator> Decorator<T>(params object[] arguments) where T : Decorator {
			Decorator decorator = (T)Activator.CreateInstance(typeof(T), arguments);
			node.SetChild(decorator);
 
			return new DecoratorBuilder<DecoratorBuilder<TParent, TNode>, Decorator>(this, decorator);
		}
		
		public LeafBuilder<DecoratorBuilder<TParent, TNode>, Leaf> Leaf<T>(params object[] arguments) where T : Leaf {
			Leaf leaf = (T)Activator.CreateInstance(typeof(T), arguments);
			node.SetChild(leaf);
			
			return new LeafBuilder<DecoratorBuilder<TParent, TNode>, Leaf>(this, leaf);
		}
		
		public DecoratorBuilder<TParent, TNode> Include(Node includeNode) {
			node.SetChild(includeNode);
			return this;
		}

		public TParent End() {
			return parent;
		}
	}
}