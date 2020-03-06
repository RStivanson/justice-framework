using System;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor {
    [Serializable]
    public class EditorBase : EditorWindow {
        [SerializeField]
        [HideInInspector]
        private Rect leftInspectorViewPort;
        
        [SerializeField]
        [HideInInspector]
        private Rect rightInspectorViewPort;

        [SerializeField]
        [HideInInspector]
        private Rect bodyViewPort;

        protected string Title {
            get { return titleContent.text; }
            set { titleContent.text = value; }
        }

        protected bool IsLeftInspectorOpen {
            get; set;
        }

        protected bool IsLeftInspectorEnabled {
            get; set;
        }

        protected string LeftInspectorTitle {
            get; set;
        }

        protected float LeftInspectorWidth {
            get; set;
        }

        protected bool IsRightInspectorOpen {
            get; set;
        }

        protected bool IsRightInspectorEnabled {
            get; set;
        }

        protected string RightInspectorTitle {
            get; set;
        }

        protected float RightInspectorWidth {
            get; set;
        }

        public void Initialize() {
            IsLeftInspectorOpen = IsLeftInspectorEnabled;
            LeftInspectorTitle = "Left Inspector";
			LeftInspectorWidth = 250f;
			leftInspectorViewPort = new Rect(0, EditorStyles.toolbar.fixedHeight, LeftInspectorWidth, position.height - EditorStyles.toolbar.fixedHeight);
			
			IsRightInspectorOpen = IsRightInspectorEnabled;
            RightInspectorTitle = "Right Inspector";
            RightInspectorWidth = 250f;
			rightInspectorViewPort = new Rect(position.width - RightInspectorWidth, EditorStyles.toolbar.fixedHeight, RightInspectorWidth, position.height - EditorStyles.toolbar.fixedHeight);
			
			bodyViewPort = new Rect(0, EditorStyles.toolbar.fixedHeight, position.width, position.height - EditorStyles.toolbar.fixedHeight);

            OnInitialize();
		}

        protected virtual void OnInitialize() {
        }
		
        private void OnGUI() {
	        UpdateViewPorts();
	        
	        GUI.Box(bodyViewPort, GUIContent.none);
	        GUILayout.BeginArea(bodyViewPort);
	        OnDrawBodyContent(new Rect(0, 0, bodyViewPort.width, bodyViewPort.height));
	        GUILayout.EndArea();
	        
	        DrawToolbar();
	        
	        if (IsLeftInspectorEnabled && IsLeftInspectorOpen) {
		        GUI.Box(leftInspectorViewPort, GUIContent.none);
		        GUILayout.BeginArea(leftInspectorViewPort);
				OnDrawInspectorLeft(new Rect(0, 0, leftInspectorViewPort.width, leftInspectorViewPort.height));
		        GUILayout.EndArea();
	        }
	        
	        if (IsRightInspectorEnabled && IsRightInspectorOpen) {
		        GUI.Box(rightInspectorViewPort, GUIContent.none);
		        GUILayout.BeginArea(rightInspectorViewPort);
		        OnDrawInspectorRight(new Rect(0, 0, rightInspectorViewPort.width, rightInspectorViewPort.height));
		        GUILayout.EndArea();
	        }
	       	
            if (GUI.changed) {
                Repaint();
            }
        }

		private void UpdateViewPorts() {
			bodyViewPort.x = 0;
			bodyViewPort.width = position.width;
			bodyViewPort.height = position.height - EditorStyles.toolbar.fixedHeight;
			
			if (IsLeftInspectorEnabled && IsLeftInspectorOpen) {
				leftInspectorViewPort.width = LeftInspectorWidth;
				leftInspectorViewPort.height = position.height - EditorStyles.toolbar.fixedHeight;

				bodyViewPort.x += LeftInspectorWidth;
				bodyViewPort.width -= LeftInspectorWidth;
			}
			
			if (IsRightInspectorEnabled && IsRightInspectorOpen) {
				rightInspectorViewPort.x = position.width - RightInspectorWidth;
				rightInspectorViewPort.width = RightInspectorWidth;
				rightInspectorViewPort.height = position.height - EditorStyles.toolbar.fixedHeight;
				
				bodyViewPort.width -= RightInspectorWidth;
			}
		}
        
#region Drawing Functions
        
        private void DrawToolbar() {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            
            OnDrawToolbarLeft();
            
            GUILayout.FlexibleSpace();
            
            OnDrawToolbarRight();
            
            GUILayout.EndHorizontal();
        }
           
	    protected virtual void OnDrawToolbarLeft() {
		    if (IsLeftInspectorEnabled && GUILayout.Button(LeftInspectorTitle, EditorStyles.toolbarButton)) {
			    IsLeftInspectorOpen = !IsLeftInspectorOpen;
		    }
	    }
	    
	    protected virtual void OnDrawToolbarRight() {
		    if (IsRightInspectorEnabled && GUILayout.Button(RightInspectorTitle, EditorStyles.toolbarButton)) {
			    IsRightInspectorOpen = !IsRightInspectorOpen;
		    }
	    }

		protected virtual void OnDrawInspectorLeft(Rect viewPort) {
		}
		
		protected virtual void OnDrawInspectorRight(Rect viewPort) {
		}
		
		protected virtual void OnDrawBodyContent(Rect viewPort) {
		}
		
#endregion
	}
}