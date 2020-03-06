using UnityEditor;

namespace JusticeFramework.Editor.Windows {
    using NodeEditor = NodeEditor.NodeEditor;

    public class BehaviourTreeNodeEditor : NodeEditor {
		[MenuItem("Justice Framework/Editor/Behaviour Editor")]
		private static void OpenWindow() {
			BehaviourTreeNodeEditor window = GetWindow<BehaviourTreeNodeEditor>();
			window.Title = "Behaviour Editor";
			window.Initialize();
		}
		
	}
}