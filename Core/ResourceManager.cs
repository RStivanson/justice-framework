using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JusticeFramework.Core {
    /// <summary>
    /// Base class that provides common functionality for all classes that need to load data for the game
    /// </summary>
	[Serializable]
	public abstract class ResourceStore<T> where T : ScriptableObject, IDataObject {
        /// <summary>
        /// Flag indicating if this manager has been initialized.
        /// </summary>
        [SerializeField]
		protected bool initialized = false;

        /// <summary>
        /// An array storing all resources.
        /// </summary>
        [SerializeField]
        protected List<T> resources;

        /// <summary>
        /// A dictionary of resources mapped by their ID.
        /// </summary>
        [SerializeField]
        protected Dictionary<string, T> resourcesById;
		
        /// <summary>
        /// Gets all of the loaded resources.
        /// </summary>
        public List<T> Resources {
            get { return resources; }
        }

        /// <summary>
        /// Gets the flag stating if this manager been initialized.
        /// </summary>
        protected bool Initialized {
            get { return initialized; }
        }

        /// <summary>
        /// Gets the amount of resources stored.
        /// </summary>
        public int Count {
            get { return resources.Count; }
        }

        /// <summary>
        /// Constructs a new resource manager.
        /// </summary>
        public ResourceStore() {
            resources = new List<T>();
            resourcesById = new Dictionary<string, T>();
        }

        /// <summary>
        /// Gets all the loaded resources.
        /// </summary>
        /// <returns>Returns a list of all loaded resources.</returns>
        public List<T> GetAll() {
            return resources;
        }

        /// <summary>
        /// Gets the given resource by its ID.
        /// </summary>
        /// <param name="id">The ID of the resource to get.</param>
        /// <returns>Returns the resource that matches the ID, or null if it doesn't exist.</returns>
		public T GetById(string id) {
			T resource;
			resourcesById.TryGetValue(id, out resource);
			return resource;
		}

        /// <summary>
        /// Gets the given resource by its ID as the specified type.
        /// </summary>
        /// <typeparam name="GetAs">The type to convert the resource to.</typeparam>
        /// <param name="id">The ID of the resource to get.</param>
        /// <returns>Returns the resource that matches the ID as the specified type, or null if it doesn't exist.</returns>
        public GetAs GetById<GetAs>(string id) where GetAs : T {
			return GetById(id) as GetAs;
		}

        /// <summary>
        /// Determines if the given resource ID has been loaded.
        /// </summary>
        /// <param name="id">The resource ID to look for.</param>
        /// <returns>Returns true if the ID exists, false otherwise</returns>
        public bool Contains(string id) {
            return GetById(id) != null;
        }

        /// <summary>
        /// Determines if the given resource ID of the specified type has been loaded.
        /// </summary>
        /// <param name="id">The resource ID to look for.</param>
        /// <returns>Returns true if the ID of the given type exists, false otherwise.</returns>
        public bool Contains<N>(string id) where N : T {
            return GetById<N>(id) != null;
        }

        /// <summary>
        /// Loads all resources from the given path and processes them.
        /// </summary>
        /// <param name="path">The path to load resources from.</param>
        /// <param name="clone">Flag indicating if the resources need cloned when loaded.</param>
		public void LoadResources(string path, bool clone = false) {
            // If this manager has not been initialized, then do nothing
            if (initialized) {
                return;
            }

            // Load all of the resources
            List<T> loadedResources = UnityEngine.Resources.LoadAll<T>(path).ToList();

            resources = loadedResources;
            resourcesById.Clear();
            
            ProcessResources(clone);

            initialized = true;
        }

        /// <summary>
        /// Processes all loaded resources.
        /// </summary>
        /// <param name="clone">Flag indicating if the loaded resources should be cloned.</param>
        protected void ProcessResources(bool clone) {
            T resource;

            // Clear any already processed data
            resourcesById = new Dictionary<string, T>();

            // Perform any pre-processing actions
            OnPreProcess();

            // Process each resource
            for (int i = 0; i < resources.Count; i++) {
                resource = resources[i];

                ProcessResource(resource, i, clone);

                // Update the progress of the process operation
                //onProgressChanged?.Invoke(false, i / resources.Count, $"Post-processing resource: {resource.Id}");
            }

            // Perform any post-processing actions
            OnPostProcess();
        }

        /// <summary>
        /// Processes an individual resources, add this to the collection of loaded resources.
        /// </summary>
        /// <param name="resource">The resource to process.</param>
        /// <param name="index">The index of the resource in the loaded array.</param>
        /// <param name="clone">Flag indicating if this resource should be cloned.</param>
        private void ProcessResource(T resource, int index, bool clone) {
            // If we want to clone the resource
            if (clone) {
                // Clone it and overwrite the original
                resource = UnityEngine.Object.Instantiate(resource);
                resources[index] = resource;
            }

            // Process the resource
            resourcesById.Add(resource.Id, resource);
            OnResourceProcessed(resource);
        }

        /// <summary>
        /// Method called before any resource processing begins.
        /// </summary>
        protected virtual void OnPreProcess() {
        }

        /// <summary>
        /// Method called after all resource processing ends.
        /// </summary>
        protected virtual void OnPostProcess() {
        }

        /// <summary>
        /// Method called each time a resource is processed.
        /// </summary>
        /// <param name="resource">The resource currently being processed.</param>
        protected virtual void OnResourceProcessed(T resource) {
        }
    }
}