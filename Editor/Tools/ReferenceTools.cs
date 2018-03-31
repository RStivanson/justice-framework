using JusticeFramework.Components;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.Tools {
	[InitializeOnLoad]
	public class ReferenceTools {
		static ReferenceTools() {
			EditorApplication.hierarchyWindowChanged -= ValidateReferenceCreateHasModel;
			EditorApplication.hierarchyWindowChanged += ValidateReferenceCreateHasModel;
		}
		
		private static void ValidateReferenceCreateHasModel() {
			GameObject activeGameObject = Selection.activeGameObject;

			if (activeGameObject == null) {
				return;
			}

			Reference referenceScript = activeGameObject.GetComponent<Reference>();

			if (referenceScript != null && !referenceScript.HasModel()) {
				Debug.LogWarning($"'{activeGameObject.name}' has no attached model object! Please attach a model script to avoid any errors");
			}
		}
	}
}