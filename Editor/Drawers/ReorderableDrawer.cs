using JusticeFramework.Core;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JusticeFramework.Editor.Drawers {
    /// <summary>
    /// Overrides the inspector drawing for readonly fields so that they are shown, but not editable.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReorderableAttribute))]
    public class ReorderableDrawer : PropertyDrawer {
        private ReorderableList list;

        /// <inheritdoc />
        /// <summary>
        /// Draws the property in the inspector.
        /// </summary>
        /// <param name="position">The position for where the property should be drawn.</param>
        /// <param name="property">The property to draw.</param>
        /// <param name="label">The label for the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            ReorderableList rl = GetReorderableList(property, attribute as ReorderableAttribute);
            rl.DoList(position);

            if (EditorGUI.EndChangeCheck()) {
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        private ReorderableList GetReorderableList(SerializedProperty property, ReorderableAttribute attribute) {
            var f = fieldInfo;
            SerializedObject obj = property.serializedObject;
            return new ReorderableList(obj, obj.FindProperty(fieldInfo.Name), attribute.Draggable, attribute.DisplayHeader, attribute.DisplayAddButton, attribute.DisplayRemoveButton);
        }
    }
}
