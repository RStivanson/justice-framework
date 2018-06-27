#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Utility {
	/// <summary>
	/// Attribute that specifies that a field is read only in the UnityEditor inspector
	/// </summary>
	public class ReadOnlyInInspectorAttribute : PropertyAttribute {
	}
 	
	/// <summary>
	/// Overrides the inspector drawing for readonly fields so that they are shown, but not editable.
	/// </summary>
	[CustomPropertyDrawer(typeof(ReadOnlyInInspectorAttribute))]
	public class ReadOnlyInInspectorDrawer : PropertyDrawer {
		/// <summary>
		/// Gets the height of given properties field
		/// </summary>
		/// <param name="property">The property to get the height for</param>
		/// <param name="label">The label content for the property</param>
		/// <returns>Returns the height of the GUI property field</returns>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
 
		/// <inheritdoc />
		/// <summary>
		/// Draws the attribute in the UnityEditor inspector
		/// </summary>
		/// <param name="position">The position for where the property should be drawn</param>
		/// <param name="property">The property to draw</param>
		/// <param name="label">The label for the property</param>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			// Handle the drawing depending on the property type
			switch (property.propertyType) {
				case SerializedPropertyType.String:
					// Draw strings as a label so the dimming of diabled fields don't hinder readability
					position = EditorGUI.PrefixLabel(position, label);
					EditorGUI.LabelField(position, property.stringValue);
					break;
				default:
					// Disable the GUI for editing, draw the field then reenabled editing
					GUI.enabled = false;
					EditorGUI.PropertyField(position, property, label, true);
					GUI.enabled = true;
					break;
			}
		}
	}
}

#endif