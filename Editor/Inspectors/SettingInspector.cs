using JusticeFramework.Core.Models.Settings;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace JusticeFramework.Editor.Inspectors {
    [CustomEditor(typeof(Setting))]
    public class SettingInspector : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            Setting setting = (Setting)target;

            GUI.enabled = !setting.IsSystemSetting;
            setting.id = EditorGUILayout.TextField("Id", setting.id);
            GUI.enabled = true;

            GUI.enabled = false;
            EditorGUILayout.IntField("Hased Id", setting.hashedId);
            GUI.enabled = true;

            setting.FloatValue = EditorGUILayout.FloatField("Float Value", setting.FloatValue);
            setting.StringValue = EditorGUILayout.TextField("String Value", setting.StringValue);

            setting.GetType().InvokeMember("OnValidate", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic, null, setting, null);
        }
    }
}