using System;

namespace JusticeFramework.Core.AI.BehaviourTree.Builder {
	public class BehaviourTreeBuilder {
		private Node root;
		
		public CompositeBuilder<BehaviourTreeBuilder, Composite> Composite<T>(params object[] arguments) where T : Composite {
			Composite composite = (T)Activator.CreateInstance(typeof(T), arguments);
			root = composite;
			
			return new CompositeBuilder<BehaviourTreeBuilder, Composite>(this, composite);
		}
		
		public DecoratorBuilder<BehaviourTreeBuilder, Decorator> Decorator<T>(params object[] arguments) where T : Decorator {
			Decorator decorator = (T)Activator.CreateInstance(typeof(T), arguments);
			root = decorator;
 
			return new DecoratorBuilder<BehaviourTreeBuilder, Decorator>(this, decorator);
		}
		
		public LeafBuilder<BehaviourTreeBuilder, Leaf> Leaf<T>(params object[] arguments) where T : Leaf {
			Leaf leaf = (T)Activator.CreateInstance(typeof(T), arguments);
			root = leaf;
			
			return new LeafBuilder<BehaviourTreeBuilder, Leaf>(this, leaf);
		}
		
		public Node Root() {
			return root;
		}
		
		public BehaviourTree Build() {
			return new BehaviourTree(root);
		}
	}
}