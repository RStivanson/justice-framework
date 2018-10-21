using UnityEditor;
using UnityEngine;
using JusticeFramework.Editor.NodeEditor;

namespace JusticeFramework.Editor.Windows {
	public class BehaviourTreeNodeEditor : NodeEditor {
		[MenuItem(EditorSettings.MenuPrefix + "/Editor/Behaviour Editor")]
		private static void OpenWindow() {
			BehaviourTreeNodeEditor window = GetWindow<BehaviourTreeNodeEditor>();
			window.titleContent = new GUIContent("Behaviour Editor");
			window.Initialize();
		}
		
	}
}