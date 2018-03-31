using JusticeFramework.Editor.NodeEditor.NBE;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor {
	public class Socket {
		protected const int OFFSET = 3;
	
		public Rect rect;
		public Node node;
		public GUIStyle style;
		public Socket connectedPoint;
		public ESocketType type;

		public Socket(Node node, ESocketType type, GUIStyle style) {
			this.node = node;
			this.type = type;
			this.style = style;
			rect = new Rect(0, 0, 10f, 20f);
		}

		public void Connect(Socket socket) {
			if (connectedPoint == socket || socket == this) {
				return;
			}

			connectedPoint?.Disconnect();
			connectedPoint = socket;
			connectedPoint.Connect(this);
		}
	
		public Socket Disconnect() {
			Socket point = connectedPoint;
			connectedPoint = null;

			return point;
		}
	
		public void DisolveConnection() {
			Socket connectedSocket = connectedPoint;
			connectedPoint = null;
			connectedSocket?.Disconnect();
		}
	
		public virtual void Draw(GraphState state, float y) {
			rect.y = y - rect.height * 0.5f;
			rect.x = node.rect.x + node.rect.width - OFFSET;

			Color def = GUI.color;
			if (connectedPoint != null) {
				GUI.color = Color.blue;
			}
			
			switch (type) {
				case ESocketType.Input:
					rect.x = node.rect.x - rect.width + OFFSET;
				
					if (GUI.Button(rect, GUIContent.none, style) && state != null) {
						if (state.selectedOutput != null) {
							Connect(state.selectedOutput);
							state.selectedOutput = null;
						} else {
							state.selectedInput = this;
						}
					}
				
					break;
				case ESocketType.Output:
					rect.x = node.rect.x + node.rect.width - OFFSET;
				
					if (GUI.Button(rect, GUIContent.none, style) && state != null) {
						if (state.selectedInput != null) {
							Connect(state.selectedInput);
							state.selectedInput = null;
						} else {
							state.selectedOutput = this;
						}
					}
				
					break;
			}
			
			GUI.color = def;
		}
	
		public void DrawConnection() {
			if (connectedPoint == null) {
				return;
			}
		
			NodeEditorUtility.DrawNodeCurve(rect, connectedPoint.rect);
			
			if (Handles.Button((rect.center + connectedPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap)) {
				connectedPoint.DisolveConnection();
			}
		}
	}
}