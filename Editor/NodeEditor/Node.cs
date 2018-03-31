using System;
using JusticeFramework.Editor.NodeEditor.NBE;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.NodeEditor {
    public delegate void OnNodeDelete(Node node);

    public class Node {
        public event OnNodeDelete OnDelete;
    
        public const int HEAD_HEIGHT = 20;
    
        public Rect rect;
        public string title;
        public bool isDragged;
        public bool isSelected;

        public Socket inPoint;
        public Socket outPoint;

        public GUIStyle style;
        public GUIStyle defaultNodeStyle;
        public GUIStyle selectedNodeStyle;

        public Action<Node> OnRemoveNode;

        public Node(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle socketStyle) {
            rect = new Rect(position.x, position.y, 200, 75);
            style = nodeStyle;
            inPoint = new Socket(this, ESocketType.Input, socketStyle);
            outPoint = new Socket(this, ESocketType.Output, socketStyle);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
        }
        
        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle socketStyle) {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            inPoint = new Socket(this, ESocketType.Input, socketStyle);
            outPoint = new Socket(this, ESocketType.Output, socketStyle);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
        }

        public bool IsClicked(Vector2 mousePosition) {
            return rect.Contains(mousePosition);
        }
    
        public void Drag(Vector2 delta) {
            rect.position += delta;
        }

        public void Draw(GraphState state) {
            float pointY = rect.y + (rect.height * 0.5f);
        
            outPoint.DrawConnection();
        
            inPoint.Draw(state, pointY);
            outPoint.Draw(state, pointY);
        
            // Draw the header
            Rect headerRect = rect;
            headerRect.height = HEAD_HEIGHT;
		
            GUI.Box(headerRect, GUIContent.none, GUI.skin.box);
            GUI.Label(headerRect, "Node");

            // Draw the node body
            Rect bodyRect = rect;
            bodyRect.y += HEAD_HEIGHT;
            bodyRect.height -= HEAD_HEIGHT;
			
            GUI.BeginGroup(bodyRect, GUI.skin.box);
            bodyRect.position = Vector2.zero;
            GUILayout.BeginArea(bodyRect);
			
            GUI.changed = false;
            OnNodeGUI();

            GUILayout.EndArea();
            GUI.EndGroup();

            GUI.Box(new Rect(rect.x, rect.y, 5, 5), "", GUI.skin.box);
            GUI.Box(new Rect(rect.x + rect.width - 5, rect.y, 5, 5), "", GUI.skin.box);
            GUI.Box(new Rect(rect.x + rect.width - 5, rect.y + rect.height - 5, 5, 5), "", GUI.skin.box);
            GUI.Box(new Rect(rect.x, rect.y + rect.height - 5, 5, 5), "", GUI.skin.box);
        }
    
        protected virtual void OnNodeGUI() {
            GUILayout.Label("Item 1");
            GUILayout.Label("Item 2");
        }

        public bool ProcessEvents(GraphState state, Event e) {
            switch (e.type) {
                case EventType.MouseDown:
                    switch (e.button) {
                        case 0:
                            if (rect.Contains(e.mousePosition)) {
                                state.selectedNode = this;
                            
                                isDragged = true;
                                isSelected = true;
                                style = selectedNodeStyle;
                                GUI.changed = true;
                            } else {
                                isSelected = false;
                                style = defaultNodeStyle;
                                GUI.changed = true;
                            }

                            break;
                        case 1:
                            if (isSelected && rect.Contains(e.mousePosition)) {
                                ProcessContextMenu();
                                e.Use();
                            }

                            break;
                    }
                
                    break;
                case EventType.MouseUp:
                    isDragged = false;
                
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged) {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }

                    break;
            }

            return false;
        }

        protected virtual void OnProcessContextMenu(GenericMenu menu) {
        }
    
        private void ProcessContextMenu() {
            GenericMenu menu = new GenericMenu();
        
            menu.AddItem(new GUIContent("Delete"), true, OnNodeDelete);
        
            if (menu.GetItemCount() > 0) {
                menu.AddSeparator(string.Empty);
            }
                
            OnProcessContextMenu(menu);

            menu.ShowAsContext();
        }

        private void OnNodeDelete() {
            inPoint.DisolveConnection();
            outPoint.DisolveConnection();
        
            OnDelete?.Invoke(this);
        }
    }
}