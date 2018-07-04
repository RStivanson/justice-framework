using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Core.Managers.Resources {
    /// <summary>
    /// Base class that provides common functionality for all classes that need to load data for the game
    /// </summary>
	[Serializable]
	public abstract class ResourceManager<T> : IOnProgressChanged where T : EntityModel {
		public event OnProgressChanged onProgressChanged;

        [SerializeField]
		protected bool initialized = false;

        [SerializeField]
        protected T[] resources;

        [SerializeField]
        protected Dictionary<string, T> resourcesById;
		
        protected bool Initialized {
            get { return initialized; }
        }

        public int ResourceCount {
            get { return resources.Length; }
        }

        public ResourceManager() {
            resources = new T[0];
            resourcesById = new Dictionary<string, T>();
        }

        public abstract void LoadResources();

		protected void LoadResources(string path, bool clone = false) {
			if (initialized) {
				return;
			}


            T[] loadedResources = UnityEngine.Resources.LoadAll<T>(path);

            if (clone) {
                for (int i = 0; i < loadedResources.Length; i++) {
                    loadedResources[i] = UnityEngine.Object.Instantiate(loadedResources[i]);
                }
            }

            resources = loadedResources;
			resourcesById.Clear();

            OnPostProcessStart();

            PostProcessResources();

            initialized = true;
        }

        protected virtual void OnPostProcessStart() {
        }

        protected void PostProcessResources() {
            T resource;
            float count = resources.Length;

            // Process each resource
            for (int i = 0; i < count; i++) {
                resource = resources[i];

                // Update the progress of the process operation
                onProgressChanged?.Invoke(false, i / count, "Post-processing resource: " + resource.id);

                // Process the resource
                resourcesById.Add(resource.id, resource);
                OnResourcePostProcessed(resource);
            }
        }

        protected virtual void OnResourcePostProcessed(T resource) {
        }
        
        protected void UpdateProgress(bool isDone, float progress, string message) {
            onProgressChanged?.Invoke(isDone, progress, message);
        }

        public T[] GetAll() {
            return resources;
        }

		public T GetById(string id) {
			T resource;
			resourcesById.TryGetValue(id, out resource);
			return resource;
		}

		public GetType GetById<GetType>(string id) where GetType : T {
			return GetById(id) as GetType;
		}
	}
}