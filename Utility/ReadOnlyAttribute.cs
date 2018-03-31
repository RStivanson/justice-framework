#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Utility {
	public class ReadOnlyInInspectorAttribute : PropertyAttribute {
	}
 
	[CustomPropertyDrawer(typeof(ReadOnlyInInspectorAttribute))]
	public class ReadOnlyInInspectorDrawer : PropertyDrawer {
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
 
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (property.propertyType == SerializedPropertyType.String) {
				position = EditorGUI.PrefixLabel(position, label);
				EditorGUI.LabelField(position, property.stringValue);
			} else {
				GUI.enabled = false;
				EditorGUI.PropertyField(position, property, label, true);
				GUI.enabled = true;
			}
		}
	}
}

#endif