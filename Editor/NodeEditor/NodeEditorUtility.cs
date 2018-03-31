using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor {
	public static class NodeEditorUtility {
		private static GUIStyle nodeStyle;
		private static GUIStyle selectedNodeStyle;
		private static GUIStyle socketStyle;

		public static GUIStyle NodeStyle {
			get {
				if (nodeStyle == null) {
					nodeStyle = new GUIStyle {
						normal = {
							background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D
						},
						border = new RectOffset(12, 12, 12, 12)
					};
				}

				return nodeStyle;
			}
		}
		
		public static GUIStyle SelectedNodeStyle {
			get {
				if (selectedNodeStyle == null) {
					selectedNodeStyle = new GUIStyle {
						normal = {
							background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D
						},
						border = new RectOffset(12, 12, 12, 12)
					};
				}

				return selectedNodeStyle;
			}
		}
		
		public static GUIStyle SocketStyle {
			get {
				if (socketStyle == null) {
					socketStyle = new GUIStyle {
						normal = {
							background = EditorGUIUtility.Load("MiniToolbarBtn") as Texture2D
						},
						active = {
							background = EditorGUIUtility.Load("MiniToolbarBtnAct") as Texture2D
						},
						border = new RectOffset(4, 4, 12, 12)
					};
				}

				return socketStyle;
			}
		}
		
		public static void DrawNodeCurve(Rect start, Rect end) {
			Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
			Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
			Vector3 startTan = startPos + Vector3.right * 50;
			Vector3 endTan = endPos + Vector3.left * 50;
			
			Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
		}
	}
}