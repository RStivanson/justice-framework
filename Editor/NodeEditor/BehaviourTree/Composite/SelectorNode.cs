using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor.BehaviourTree.Composite {
	[NodeMenuItem("Behaviour Tree/Composite/Selector")]
	public class SelectorNode : Node {
		public SelectorNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle socketStyle) : base(position, nodeStyle, selectedStyle, socketStyle) { }

		protected override void OnNodeGUI() {
			GUILayout.TextField("Label");
			GUILayout.TextField("Label2");
		}
	}
}