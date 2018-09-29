#if UNITY_EDITOR

using JusticeFramework.Components;
using JusticeFramework.Core;
using JusticeFramework.Core.Factions;
using JusticeFramework.Core.Models;
using JusticeFramework.Core.Models.Crafting;
using JusticeFramework.Core.Models.Dialogue;
using JusticeFramework.Core.Models.Quest;
using JusticeFramework.Core.Models.Settings;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JusticeFramework.Editor.Tools {
    /// <summary>
    /// Handles all editor functions relating to SciptableObjects
    /// </summary>
    public static class ScriptableObjectTools {
		/// <summary>
		/// Creates a ScriptableObject of the given type with a default name
		/// </summary>
		/// <typeparam name="T">The type of object to create</typeparam>
		/// <returns>Returns an instance of the instantiated type, null if it fails</returns>
		private static T CreateScriptableObject<T> () where T : ScriptableObject {
			return CreateScriptableObject<T>(
				AssetDatabase.GetAssetPath(Selection.activeObject),
				"New " + typeof(T).Name
			);
		}

		/// <summary>
		/// Creates a SciptableObject of the given type of the specified path and name
		/// </summary>
		/// <param name="path">Path to the object model</param>
		/// <param name="name">Name of the object model</param>
		/// <typeparam name="T">The type of object to create</typeparam>
		/// <returns>Returns an instance of the instantiatied model, null if it fails</returns>
		public static T CreateScriptableObject<T>(string path, string name) where T : ScriptableObject {
			// Create a default instance of the ScriptableObject
			T asset = ScriptableObject.CreateInstance<T>();

			// If the path is null or empty
			if (string.IsNullOrEmpty(path)) {
				// Check the base Assets folder
				path = "Assets";
			}


			// If the path has an extension
			if (!Path.GetExtension(path).Equals(string.Empty)) {
				// Replace the file name in the path with the empty string
				path = path.Replace(
					Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)),
					string.Empty
				);
			}

			// Generate a unique path for the new object
			// ex. if "Rules.xml" exists it will return "Rules 1.xml"
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");
			
			// Create the asset and save the changes to the database
			AssetDatabase.CreateAsset(asset, assetPathAndName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			
			// Focus the project window so the user can easily access the new asset
			Selection.SetActiveObjectWithContext(asset, null);
			UnityEditor.EditorUtility.FocusProjectWindow();
			
			return asset;
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Activator")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Activator")]
        public static void CreateActivatorData() {
            CreateScriptableObject<ActivatorModel>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Actor")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Actor")]
        public static void CreateActorData() {
            CreateScriptableObject<ActorModel>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Ammo")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Ammo")]
        public static void CreateAmmoData() {
            CreateScriptableObject<AmmoModel>();
        }

		[MenuItem(EditorSettings.MenuPrefix + "/Create/Create Armor")]
		[MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Armor")]
		public static void CreateArmorData() {
			CreateScriptableObject<ArmorModel>();
		}

		[MenuItem(EditorSettings.MenuPrefix + "/Create/Create Codex")]
		[MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Codex")]
		public static void CreateCodexData() {
			CreateScriptableObject<CodexModel>();
		}

		[MenuItem(EditorSettings.MenuPrefix + "/Create/Create Consumable")]
		[MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Consumable")]
		public static void CreateConsumableData() {
			CreateScriptableObject<ConsumableModel>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Conversation")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Conversation")]
        public static void CreateConversation() {
            CreateScriptableObject<Conversation>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Chest")]
		[MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Chest")]
		public static void CreateChestData() {
			CreateScriptableObject<ChestModel>();
		}

		[MenuItem(EditorSettings.MenuPrefix + "/Create/Create Door")]
		[MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Door")]
		public static void CreateDoorData() {
			CreateScriptableObject<DoorModel>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Faction")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Faction")]
        public static void CreateFaction() {
            CreateScriptableObject<Faction>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Flower")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Flower")]
        public static void CreateFlowerData() {
            CreateScriptableObject<FlowerModel>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Item")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Item")]
        public static void CreateItemData() {
            CreateScriptableObject<ItemModel>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Merchant Inventory")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Merchant Inventory")]
        public static void CreateMerchantInventory() {
            CreateScriptableObject<MerchantInventory>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Quest")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Quest")]
        public static void CreateQuest() {
            CreateScriptableObject<Quest>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Recipe")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Recipe")]
        public static void CreateRecipeData() {
            CreateScriptableObject<Recipe>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Setting")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Setting")]
        public static void CreateSetting() {
            CreateScriptableObject<GameSetting>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Status Effect")]
        [MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Status Effect")]
        public static void CreateStatusEffect() {
            CreateScriptableObject<StatusEffectModel>();
        }

        [MenuItem(EditorSettings.MenuPrefix + "/Create/Create Weapon")]
		[MenuItem("Assets/" + EditorSettings.MenuPrefix + "/Create/Create Weapon")]
		public static void CreateWeaponData() {
			CreateScriptableObject<WeaponModel>();
		}

		[MenuItem("Assets/Send To Scene")]
		public static void SendToScene() {
			WorldObjectModel worldObject = Selection.activeObject as WorldObjectModel;
			Transform lastSceneViewCameraTransform = SceneView.lastActiveSceneView.camera.transform;
			RaycastHit hit;
			Vector3 spawnPoint;

			if (Physics.Raycast(lastSceneViewCameraTransform.position, lastSceneViewCameraTransform.forward, out hit)) {
				spawnPoint = hit.point;
				spawnPoint.y += 0.025f;
			} else {
				spawnPoint = lastSceneViewCameraTransform.position + (lastSceneViewCameraTransform.forward * 10.0f);
			}

			WorldObject reference = ((GameObject)PrefabUtility.InstantiatePrefab(worldObject.groundItemPrefab)).GetComponent<WorldObject>();

			reference.transform.position = spawnPoint;
			reference.transform.rotation = Quaternion.identity;

			Undo.RecordObject(reference, "Send To Scene Command");
			
			reference.SetData(worldObject, false);
		}
		
		[MenuItem("Assets/Send To Scene", true)]
		private static bool SendToSceneValidation() {
			WorldObjectModel worldObject = Selection.activeObject as WorldObjectModel;
			
			bool hasPrefab = worldObject?.groundItemPrefab != null;
			bool prefabHasReferenceScript = worldObject?.groundItemPrefab?.GetComponent<WorldObject>() != null;
			
			return hasPrefab && prefabHasReferenceScript;
		}
	}
}

#endif