#if UNITY_EDITOR

using JusticeFramework.Components;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.Tools {
    /// <summary>
    /// Handles all editor functions relating to SciptableObjects
    /// </summary>
    public static class ScriptableObjectTools {
        private const string DefaultAssetPath = "Resources/Data";

        /// <summary>
        /// Creates a ScriptableObject of the given type with a default name
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <returns>Returns an instance of the instantiated type, null if it fails</returns>
        public static T CreateScriptableObject<T>() where T : ScriptableObject {
            return CreateScriptableObject(ScriptableObject.CreateInstance<T>(), DefaultAssetPath, $"New {typeof(T).Name}");
		}

        /// <summary>
        /// Creates a ScriptableObject of the given type with a default name
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="path">The path to create the new scriptable object at</param>
        /// <param name="name">Name of the object model</param>
        /// <returns>Returns an instance of the instantiated type, null if it fails</returns>
        public static T CreateScriptableObject<T>(string path, string name) where T : ScriptableObject {
            return CreateScriptableObject(ScriptableObject.CreateInstance<T>(), path, name);
        }

        /// <summary>
        /// Creates a ScriptableObject of the given type with a default name
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="path">The path to create the new scriptable object at</param>
        /// <returns>Returns an instance of the instantiated type, null if it fails</returns>
        public static T CreateScriptableObject<T>(T asset, string path) where T : ScriptableObject {
            return CreateScriptableObject(asset, path, $"New {typeof(T).Name}");
        }

        /// <summary>
        /// Creates a SciptableObject asset using the data passed at the specified path with the given name
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="asset">The asset to add to the asset database</param>
        /// <param name="path">Path to the object model</param>
        /// <param name="name">Name of the object model</param>
        /// <returns>Returns an instance of the instantiatied model, null if it fails</returns>
        public static T CreateScriptableObject<T>(T asset, string path, string name) where T : ScriptableObject {
            // If the path is null or empty
            if (string.IsNullOrEmpty(path)) {
                if (Selection.activeObject != null) {
                    path = AssetDatabase.GetAssetPath(Selection.activeObject);
                } else {
                    path = DefaultAssetPath;
                }
            }

            // If the path has an extension
            if (!Path.GetExtension(path).Equals(string.Empty)) {
                // Replace the file name in the path with the empty string
                string pathName = Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
                path = path.Replace(pathName, string.Empty);
            }

            if (!path.StartsWith("Assets")) {
                path = "Assets/" + path;
            }

            // Generate a unique path for the new object
            // ex. if "Rules.xml" exists it will return "Rules 1.xml"
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/{name}.asset");

            // Create the asset and save the changes to the database
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Focus the project window so the user can easily access the new asset
            //Selection.SetActiveObjectWithContext(asset, null);
            //UnityEditor.EditorUtility.FocusProjectWindow();

            return asset;
        }

        public static T Create<T>(T asset, string path) where T : ScriptableObject {
            return CreateScriptableObject(asset, path);
        }

		[MenuItem("Assets/Send To Scene")]
		public static void SendToScene() {
			ScriptableDataObject worldObject = Selection.activeObject as ScriptableDataObject;
			Transform lastSceneViewCameraTransform = SceneView.lastActiveSceneView.camera.transform;
			RaycastHit hit;
			Vector3 spawnPoint;

			if (Physics.Raycast(lastSceneViewCameraTransform.position, lastSceneViewCameraTransform.forward, out hit)) {
				spawnPoint = hit.point;
				spawnPoint.y += 0.025f;
			} else {
				spawnPoint = lastSceneViewCameraTransform.position + (lastSceneViewCameraTransform.forward * 10.0f);
			}

            ISceneDataObject sceneObject = worldObject as ISceneDataObject;
			WorldObject reference = ((GameObject)PrefabUtility.InstantiatePrefab(sceneObject.ScenePrefab)).GetComponent<WorldObject>();

			reference.transform.position = spawnPoint;
			reference.transform.rotation = Quaternion.identity;

			Undo.RecordObject(reference, "Send To Scene Command");
			
			reference.SetData(worldObject);
		}
		
		[MenuItem("Assets/Send To Scene", true)]
		private static bool SendToSceneValidation() {
            ScriptableDataObject worldObject = Selection.activeObject as ScriptableDataObject;
            ISceneDataObject sceneObject = worldObject as ISceneDataObject;

            bool hasPrefab = sceneObject?.ScenePrefab != null;
			bool prefabHasReferenceScript = sceneObject?.ScenePrefab?.GetComponent<WorldObject>() != null;
			
			return hasPrefab && prefabHasReferenceScript;
		}
	}
}

#endif