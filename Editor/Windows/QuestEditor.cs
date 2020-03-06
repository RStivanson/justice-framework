using JusticeFramework.Data;
using JusticeFramework.Editor;
using JusticeFramework.Editor.Tools;
using JusticeFramework.Managers.Resources;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.JusticeFramework.Editor.Windows {
    /*public class QuestEditor : EditorBase {
        private const string QuestAssetPath = "Data/Quests/";
        private QuestManager questManager;
        private QuestData activeQuest;

        private List<QuestData.QuestStage> stages;
        private bool isNew;

        [MenuItem("Justice Framework/Editor/Quest Editor")]
        private static void OpenWindow() {
            QuestEditor window = GetWindow<QuestEditor>();
            window.Title = "Quest Editor";
            window.Initialize();
        }

        protected override void OnInitialize() {
            IsLeftInspectorEnabled = true;
            IsLeftInspectorOpen = true;

            questManager = new QuestManager();
            questManager.LoadResources(QuestAssetPath);

            SelectQuest(null, false);
        }

        private void SelectQuest(QuestData quest, bool isNew) {
            activeQuest = quest;
            this.isNew = isNew;

            if (activeQuest == null) {
                activeQuest = null;
                stages = new List<QuestData.QuestStage>();
            } else {
                stages = activeQuest.Stages.ToList();
            }
        }

        #region UI Functions

        protected override void OnDrawToolbarLeft() {
            base.OnDrawToolbarLeft();

            if (GUILayout.Button(new GUIContent("New Quest"), EditorStyles.toolbarButton)) {
                activeQuest = CreateInstance<QuestData>();
                SelectQuest(activeQuest, true);
            }

            if (activeQuest != null) {
                if (GUILayout.Button(new GUIContent("Save"), EditorStyles.toolbarButton)) {
                    if (isNew) {
                        //activeQuest.systemId = new Guid();
                        ScriptableObjectTools.Create(activeQuest, QuestAssetPath);
                    }

                    if (!questManager.Contains(activeQuest.Id)) {
                        questManager.Register(activeQuest);
                    }

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                if (GUILayout.Button(new GUIContent("Close Quest"), EditorStyles.toolbarButton)) {
                    if (isNew) {
                        Destroy(activeQuest);
                    }

                    activeQuest = null;
                }
            }
        }

        protected override void OnDrawInspectorLeft(Rect viewPort) {
            EditorGUILayout.BeginVertical();

            foreach (QuestData quest in questManager.Resources) {
                if (GUILayout.Button(new GUIContent(quest.Id))) {
                    SelectQuest(quest, false);
                }
            }

            EditorGUILayout.EndVertical();
        }
        
        protected override void OnDrawBodyContent(Rect viewPort) {
            UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(activeQuest);
            editor.OnInspectorGUI();
        }

        #endregion
    }*/
}
