using System;
using System.Collections.Generic;
using JusticeFramework.Data.Models;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework {
	[Serializable]
	public class AssetManager : IOnProgressChanged {
		[SerializeField]
		private bool initialized = false;

		[SerializeField]
		private WorldObjectModel[] worldObjects;

		[SerializeField]
		private Dictionary<string, WorldObjectModel> entitiesById;

		[SerializeField]
		private Dictionary<Type, List<WorldObjectModel>> entitiesByType;

		public event OnProgressChanged onProgressChanged;
		
		public int Count {
			get {
				return worldObjects.Length;
			}
		}

		public AssetManager() {
			worldObjects = new WorldObjectModel[0];
			entitiesById = new Dictionary<string, WorldObjectModel>();
			entitiesByType = new Dictionary<Type, List<WorldObjectModel>>();
		}

		public void LoadAssets() {
			if (initialized) {
				return;
			}

			initialized = true;

			if (onProgressChanged != null) {
				onProgressChanged(false, 0, "Loading worldObject definitions");
			}

			worldObjects = Resources.LoadAll<WorldObjectModel>("Data/AssetData/");
			entitiesById = new Dictionary<string, WorldObjectModel>();
			entitiesByType = new Dictionary<Type, List<WorldObjectModel>>();
			
			for (int i = 0; i < worldObjects.Length; ++i) {
				if (onProgressChanged != null) {
					onProgressChanged(false, 0.2f + (0.8f * (i / (float)worldObjects.Length)), "Post-processing worldObject: " + worldObjects[i].id);
				}

				List<WorldObjectModel> entitiesOfType;
					
				if (entitiesByType.TryGetValue(worldObjects[i].GetType(), out entitiesOfType)) {
					entitiesOfType.Add(worldObjects[i]);
				} else {
					entitiesOfType = new List<WorldObjectModel>();
					entitiesOfType.Add(worldObjects[i]);

					entitiesByType.Add(worldObjects[i].GetType(), entitiesOfType);
				}

				entitiesById.Add(worldObjects[i].id, worldObjects[i]);
			}

			if (onProgressChanged != null) {
				onProgressChanged(true, 1.0f, "Done");
			}
		}
		
		public WorldObjectModel GetEntityById(string id) {
			WorldObjectModel worldObject;

			entitiesById.TryGetValue(id, out worldObject);

			return worldObject;
		}

		public T GetEntityById<T>(string id) where T : WorldObjectModel {
			WorldObjectModel worldObject;

			entitiesById.TryGetValue(id, out worldObject);

			return worldObject as T;
		}

		public List<T> GetDefinitionsOfType<T>() where T : WorldObjectModel {
			List<WorldObjectModel> internalEntities;
			List<T> entitiesOfType = new List<T>();

			entitiesByType.TryGetValue(typeof(T), out internalEntities);
			
			for (int i = 0; i < internalEntities.Count; ++i) {
				entitiesOfType.Add((T)internalEntities[i]);
			}

			return entitiesOfType;
		}
	}
}