using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor.BehaviourTree.Composite {
	[NodeMenuItem("Behaviour Tree/Composite/Sequence")]
	public class SequenceNode : Node {
		public SequenceNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle socketStyle) : base(position, nodeStyle, selectedStyle, socketStyle) {
			rect.height = 200f;
		}
		
		protected override void OnNodeGUI() {
			GUILayout.TextField("Label");
		}
	}
}