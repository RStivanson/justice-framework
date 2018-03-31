using JusticeFramework.Editor.NodeEditor.NBE;
using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor {
	public class GraphState {
		public Node selectedNode;
		public Socket selectedInput;
		public Socket selectedOutput;
		public Vector2 drag;

		public GraphState() {
			selectedNode = null;
			selectedInput = null;
			selectedOutput = null;
			drag = Vector2.zero;
		}
	}
}