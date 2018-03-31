using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.Utility {
	public static class EditorDraw {
		public static void DrawBezier(Vector2 start, Vector2 end, Color color) {
			Vector2 endToStart = (end - start);
			float dirFactor = Mathf.Clamp(endToStart.magnitude, 20f, 80f);

			endToStart.Normalize();
			Vector2 project = Vector3.Project(endToStart, Vector3.right);

			Vector2 startTan = start + project * dirFactor;
			Vector2 endTan = end - project * dirFactor;

			Handles.DrawBezier(start, end, startTan, endTan, color, null, 3f);
		}

		public static void DrawLine(Vector2 start, Vector2 end, Color color) {
			Color handleColor = Handles.color;
			Handles.color = color;

			Handles.DrawLine(start, end);
			Handles.color = handleColor;
		}
	}
}