namespace JusticeFramework.Data.AI.BehaviourTree {
	public abstract class BehaviourSet {
		private readonly BehaviourTree tree;
		
		protected BehaviourSet() {
			tree = BuildBehaviourSet();
		}

		public void Tick(TickState tick) {
			tree.Tick(tick);
		}

		protected abstract BehaviourTree BuildBehaviourSet();
	}
}