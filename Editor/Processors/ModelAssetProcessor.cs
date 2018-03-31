#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace JusticeFramework.Editor.Processors {
	public class ModelAssetProcessor : AssetPostprocessor {
		private void OnPreprocessModel() {
			// Only operate on FBX files
			if (!HasExtension(assetPath, ".blend") && !HasExtension(assetPath, ".fbx")) {
				return;
			}

			ModelImporter modelImport = (ModelImporter)assetImporter;

			modelImport.importLights = false;
			modelImport.importCameras = false;
			modelImport.importVisibility = false;

			if (modelImport.clipAnimations != null && modelImport.clipAnimations.Length == 0) {
				modelImport.importAnimation = false;
			}
		}

		private bool HasExtension(string path, string extension) {
			return assetPath.IndexOf(extension, StringComparison.InvariantCulture) != -1;
		}
	}
}

#endif