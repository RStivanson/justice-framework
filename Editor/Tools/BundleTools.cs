#if UNITY_EDITOR

using UnityEditor;

namespace JusticeFramework.Editor.Tools {
	/// <summary>
	/// Handles all static editor functions relating to Unity asset bundles
	/// </summary>
	public static class BundleTools {
		/// <summary>
		/// Builds all specified bundles in the editor
		/// </summary>
		[MenuItem(EditorSettings.MenuPrefix + "/Tools/Build AssetBundles")]
		[MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Tools/Build AssetBundles")]
		private static void BuildAllAssetBundles() {
			BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
		}
	}
}

#endif