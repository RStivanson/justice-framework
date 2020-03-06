using System.Collections.Generic;

namespace JusticeFramework.Core.AI.BehaviourTree {
	public class Blackboard {
		private readonly Dictionary<object, object> memory;

		public Blackboard() {
			memory = new Dictionary<object, object>();
		}

		private Dictionary<object, object> GetNodeMemory(object nodeId) {
			if (!memory.ContainsKey(nodeId)) {
				memory.Add(nodeId, new Dictionary<object, object>());
			}
			
			return memory[nodeId] as Dictionary<object, object>;
		}

		private Dictionary<object, object> GetMemory(object nodeId) {
			return nodeId == null ? memory : GetNodeMemory(nodeId);
		}
		
		public void Set(object key, object value, object nodeId = null) {
			Dictionary<object, object> setMemory = GetMemory(nodeId);

			if (!setMemory.ContainsKey(key)) {
				setMemory.Add(key, value);
			} else {
				setMemory[key] = value;
			}
		}

		public object Get(object key, object nodeId = null) {
			object value = null;
			GetMemory(nodeId).TryGetValue(key, out value);
			return value;
		}
		
		public T Get<T>(object key, object nodeId = null) {
			object value = Get(key, nodeId);
			return (value != null) ? (T)value : default(T);
		}
	}
}