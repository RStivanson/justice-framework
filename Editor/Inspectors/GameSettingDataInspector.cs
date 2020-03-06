using JusticeFramework.Data;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.Inspectors {
    [CustomEditor(typeof(GameSettingData))]
    public class GameSettingDataInspector : UnityEditor.Editor {
        SerializedProperty fValue;
        SerializedProperty sValue;

        private void OnEnable() {
            fValue = serializedObject.FindProperty("floatValue");
            sValue = serializedObject.FindProperty("stringValue");
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(fValue, new GUIContent("Float Value"));
            EditorGUILayout.PropertyField(sValue, new GUIContent("String Value"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}