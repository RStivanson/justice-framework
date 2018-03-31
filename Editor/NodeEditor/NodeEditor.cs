using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor {
    public class NodeEditor : EditorBase {
        private Vector2 offset;
        private Graph graph;

        private void OnEnable() {
            graph = new Graph();
        }
        
        protected override void OnDrawBodyContent(Rect viewPort) {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            graph.Draw();
            graph.ProcessEvents(Event.current);
            
            if (GUI.changed) {
                Repaint();
            }
        }
        
        protected override void OnDrawToolbarLeft() {
            base.OnDrawToolbarLeft();
            
            if (GUILayout.Button("Reset Offset", EditorStyles.toolbarButton)) {
                graph.AddDrag(-offset);
                offset = Vector2.zero;
            }
        }
        
#region Drawing Functions

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor) {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
        
            gridColor.a = gridOpacity;
            Handles.color = gridColor;

            offset += graph.State.drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++) {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++) {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

#endregion
    }
}