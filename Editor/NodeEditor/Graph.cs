using System;
using System.Collections.Generic;
using System.Reflection;
using JusticeFramework.Components;
using JusticeFramework.Utility.Extensions;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor {
	[Serializable]
	public class Graph {
		[Serializable]
		private class NodeData {
			public string path;
			public Type type;
		}
		
		[SerializeField]
		protected List<Node> nodes;
		
		[SerializeField]
		private GraphState state;
		
		[SerializeField]
		private List<NodeData> nodeTypes;
		
		public GraphState State {
			get { return state; }
		}
		
		public Graph() {
			nodes = new List<Node>();
			state = new GraphState();
			LoadNodeTypes();
		}

#region Drawing

		public void Draw() {
			DrawNodes();
			DrawConnectionLine(Event.current);
		}

		public void Draw(Rect viewPort) {
			DrawNodes(viewPort);
			DrawConnectionLine(Event.current);
		}
		
		private void DrawNodes() {
			if (nodes == null) {
				return;
			}
	
			foreach (Node node in nodes) {
				node.Draw(state);
			}
		}
		
		private void DrawNodes(Rect viewPort) {
			if (nodes == null) {
				return;
			}
	
			foreach (Node node in nodes) {
				if (viewPort.Overlaps(node.rect)) {
					node.Draw(state);
				}
			}
		}
		
		private void DrawConnectionLine(Event e) {
			Socket startSocket = state.selectedInput ?? state.selectedOutput;
	
			if (startSocket == null) {
				return;
			}
	
			NodeEditorUtility.DrawNodeCurve(startSocket.rect, new Rect(e.mousePosition, Vector2.zero));
			
			GUI.changed = true;
		}
		
#endregion

#region Processing

		public void ProcessEvents(Event e) {
			ProcessNodeEvents(e);
			ProcessGraphEvents(e);
		}

		private void ProcessNodeEvents(Event e) {
			if (nodes == null) {
				return;
			}

			for (int i = nodes.Count - 1; i >= 0; i--) {
				if (nodes[i].ProcessEvents(state, e)) {
					GUI.changed = true;
				}
			}
		}

		private void ProcessGraphEvents(Event e) {
			state.drag = Vector2.zero;

			switch (e.type) {
				case EventType.MouseDown:
					if (e.button == 0) {
						state.selectedInput = null;
						state.selectedOutput = null;
					}

					if (e.button == 1) {
						ProcessContextMenu(e.mousePosition);
					}

					break;
				case EventType.MouseDrag:
					if (e.button == 0) {
						OnDrag(e.delta);
					}

					break;
			}
		}
		
		private void OnDrag(Vector2 delta) {
			state.drag = delta;

			if (nodes != null) {
				foreach (Node node in nodes) {
					node.Drag(state.drag);
				}
			}

			GUI.changed = true;
		}

		public void AddDrag(Vector2 offset) {
			foreach (Node node in nodes) {
				node.Drag(offset);
			}
		}

		private void ProcessContextMenu(Vector2 mousePosition) {
			GenericMenu contextMenu = new GenericMenu();
			
			foreach (NodeData node in nodeTypes) {
				contextMenu.AddItem(new GUIContent(node.path), false, () => {
					CreateNode(mousePosition, node.type);
				});
			}

			OnProcessContextMenu(contextMenu, mousePosition);
        
			if (contextMenu.GetItemCount() > 0) {
				contextMenu.ShowAsContext();
			}
		}
		
		protected virtual void OnProcessContextMenu(GenericMenu menu, Vector2 mousePostion) {
		}

		private void LoadNodeTypes() {
			nodeTypes = new List<NodeData>();
			
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
				foreach (Type type in assembly.GetTypes()) {
					foreach (Attribute attribute in type.GetCustomAttributes(true)) {
						if (attribute.NotType<NodeMenuItem>()) {
							continue;
						}

						NodeMenuItem item = (NodeMenuItem)attribute;
						nodeTypes.Add(new NodeData {
							path = item.Path,
							type = type
						});
					}
				}
			}
		}
		
		private void CreateNode(Vector2 pos, Type nodeType) {
			Node node = (Node)System.Activator.CreateInstance(nodeType, pos, NodeEditorUtility.NodeStyle, NodeEditorUtility.SelectedNodeStyle, NodeEditorUtility.SocketStyle);
			nodes.Add(node);
		}

#endregion
	}
}