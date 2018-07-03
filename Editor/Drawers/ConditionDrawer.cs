using JusticeFramework.Core;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.Drawers {
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionDrawer : PropertyDrawer {
        private const float PropertyBuffer = 5;
        private const float SelfFlagWidth = 15;
        private const float TargetWidth = 135;
        private const float EqualityWidth = 75;
        private const float StringWidth = 130;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect selfRect = new Rect(position.x, position.y, SelfFlagWidth, position.height);
            Rect conditionRect = new Rect(position.x + (SelfFlagWidth + PropertyBuffer), position.y, TargetWidth, position.height);
            Rect equalityRect = new Rect(position.x + (SelfFlagWidth + PropertyBuffer + TargetWidth + PropertyBuffer), position.y, EqualityWidth, position.height);
            Rect stringRect = new Rect(position.x + (SelfFlagWidth + PropertyBuffer + TargetWidth + PropertyBuffer + EqualityWidth + PropertyBuffer), position.y, StringWidth, position.height);
            Rect floatRect = new Rect(position.x + (SelfFlagWidth + PropertyBuffer + TargetWidth + PropertyBuffer + EqualityWidth + PropertyBuffer + StringWidth + PropertyBuffer), position.y, position.width - (SelfFlagWidth + PropertyBuffer + TargetWidth + PropertyBuffer + EqualityWidth + PropertyBuffer + StringWidth + PropertyBuffer), position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(selfRect, property.FindPropertyRelative("targetSelf"), GUIContent.none);
            EditorGUI.PropertyField(conditionRect, property.FindPropertyRelative("conditionMethod"), GUIContent.none);
            EditorGUI.PropertyField(equalityRect, property.FindPropertyRelative("equalityType"), GUIContent.none);
            EditorGUI.PropertyField(stringRect, property.FindPropertyRelative("stringValue"), GUIContent.none);
            EditorGUI.PropertyField(floatRect, property.FindPropertyRelative("floatValue"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}