using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor.BehaviourTree {
	public class BehaviourTreeNodeEditor : NodeEditor {
		[MenuItem(EditorSettings.MenuPrefix + "/Editor/Behaviour Editor")]
		private static void OpenWindow() {
			BehaviourTreeNodeEditor window = GetWindow<BehaviourTreeNodeEditor>();
			window.titleContent = new GUIContent("Behaviour Editor");
			window.Initialize();
		}
		
	}
}