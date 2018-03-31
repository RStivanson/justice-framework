using System;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor {
	[Serializable]
	public class EditorBase : EditorWindow {
		[SerializeField]
		private bool isLeftInspectorOpen;

		[SerializeField]
		private float leftInspectorWidth;
		
		[SerializeField]
		[HideInInspector]
		private Rect leftInspectorViewPort;
		
		[SerializeField]
		private bool isRightInspectorOpen;
		
		[SerializeField]
		private float rightInspectorWidth;

		[SerializeField]
		[HideInInspector]
		private Rect rightInspectorViewPort;
		
		[SerializeField]
		[HideInInspector]
		private Rect bodyViewPort;
		
		protected virtual void Initialize() {
			isLeftInspectorOpen = true;
			leftInspectorWidth = 250f;
			leftInspectorViewPort = new Rect(0, EditorStyles.toolbar.fixedHeight, leftInspectorWidth, position.height - EditorStyles.toolbar.fixedHeight);
			
			isRightInspectorOpen = true;
			rightInspectorWidth = 250f;
			rightInspectorViewPort = new Rect(position.width - rightInspectorWidth, EditorStyles.toolbar.fixedHeight, rightInspectorWidth, position.height - EditorStyles.toolbar.fixedHeight);
			
			bodyViewPort = new Rect(0, EditorStyles.toolbar.fixedHeight, position.width, position.height - EditorStyles.toolbar.fixedHeight);
		}
		
        private void OnGUI() {
	        UpdateViewPorts();
	        
	        GUI.Box(bodyViewPort, GUIContent.none);
	        GUILayout.BeginArea(bodyViewPort);
	        OnDrawBodyContent(new Rect(0, 0, bodyViewPort.width, bodyViewPort.height));
	        GUILayout.EndArea();
	        
	        DrawToolbar();
	        
	        if (isLeftInspectorOpen) {
		        GUI.Box(leftInspectorViewPort, GUIContent.none);
		        GUILayout.BeginArea(leftInspectorViewPort);
				OnDrawInspectorLeft(new Rect(0, 0, leftInspectorViewPort.width, leftInspectorViewPort.height));
		        GUILayout.EndArea();
	        }
	        
	        if (isRightInspectorOpen) {
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
			
			if (isLeftInspectorOpen) {
				leftInspectorViewPort.width = leftInspectorWidth;
				leftInspectorViewPort.height = position.height - EditorStyles.toolbar.fixedHeight;

				bodyViewPort.x += leftInspectorWidth;
				bodyViewPort.width -= leftInspectorWidth;
			}
			
			if (isRightInspectorOpen) {
				rightInspectorViewPort.x = position.width - rightInspectorWidth;
				rightInspectorViewPort.width = rightInspectorWidth;
				rightInspectorViewPort.height = position.height - EditorStyles.toolbar.fixedHeight;
				
				bodyViewPort.width -= rightInspectorWidth;
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
		    if (GUILayout.Button("Left btn", EditorStyles.toolbarButton)) {
			    isLeftInspectorOpen = !isLeftInspectorOpen;
		    }
	    }
	    
	    protected virtual void OnDrawToolbarRight() {
		    if (GUILayout.Button("Right btn", EditorStyles.toolbarButton)) {
			    isRightInspectorOpen = !isRightInspectorOpen;
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