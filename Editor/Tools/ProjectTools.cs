using UnityEditor;

namespace JusticeFramework.Editor.Tools {
	public class ProjectUtility {
		[MenuItem(EditorSettings.MenuPrefix + "/Project/Initialize New...", false, 10)]
		public static void InitializeNewProject() {
			string dialogMessage = "You are about to initialize a new project. This will create folders in your project spaces and set up the editor. Do you wish to continue?";
			
			if (!UnityEditor.EditorUtility.DisplayDialog("New Project", dialogMessage, "Yes", "No")) {
				return;
			}

			// Resources
			CreateResourceFolders();
			
			// Scenes
			AssetDatabase.CreateFolder("Assets", "Scenes");
			
			// Scripts
			AssetDatabase.CreateFolder("Assets", "Scripts");
		}
		
		private static void CreateResourceFolders() {
			AssetDatabase.CreateFolder("Assets", "Resources");
			
			// Audio
			AssetDatabase.CreateFolder("Assets/Resources", "Audio");
			AssetDatabase.CreateFolder("Assets/Resources/Audio", "SFX");
			AssetDatabase.CreateFolder("Assets/Resources/Audio", "Music");
			AssetDatabase.CreateFolder("Assets/Resources/Audio", "Voice");
			
			// Data
			AssetDatabase.CreateFolder("Assets/Resources", "Data");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Actors");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Default");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Dialogue");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Doors");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Items");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Factions");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Static");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "Quests");
			AssetDatabase.CreateFolder("Assets/Resources/Data", "UI");
			
			// Fonts
			AssetDatabase.CreateFolder("Assets/Resources", "Fonts");
			
			// Models
			AssetDatabase.CreateFolder("Assets/Resources", "Models");
			AssetDatabase.CreateFolder("Assets/Resources/Models", "Materials");
			
			// Plugins
			AssetDatabase.CreateFolder("Assets/Resources", "Plugins");
			
			// Prefabs
			AssetDatabase.CreateFolder("Assets/Resources", "Prefabs");
			AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Actors");
			AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Default");
			AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Doors");
			AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Items");
			AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "Static");
			AssetDatabase.CreateFolder("Assets/Resources/Prefabs", "UI");
			
			// Sprites
			AssetDatabase.CreateFolder("Assets/Resources", "Sprites");
		}
	}
}