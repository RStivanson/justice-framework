using JusticeFramework.Core.Models.Settings;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.Inspectors {
    [CustomEditor(typeof(Setting))]
    public class SettingInspector : UnityEditor.Editor {
        SerializedProperty floatValueProp;
        SerializedProperty stringValueProp;

        private void OnEnable() {
            floatValueProp = serializedObject.FindProperty("floatValue");
            stringValueProp = serializedObject.FindProperty("stringValue");
        }

        public override void OnInspectorGUI() {
            Setting setting = (Setting)target;

            GUI.enabled = !setting.IsSystemSetting;
            setting.id = EditorGUILayout.TextField("Id", setting.id);
            GUI.enabled = true;

            GUI.enabled = false;
            EditorGUILayout.IntField("Hased Id", setting.hashedId);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(floatValueProp, new GUIContent("Float Value"));
            EditorGUILayout.PropertyField(stringValueProp, new GUIContent("String Value"));
        }
    }
}