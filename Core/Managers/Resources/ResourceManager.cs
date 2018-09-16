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

        /// <summary>
        /// Flag indicating if this manager has been initialized
        /// </summary>
        [SerializeField]
		protected bool initialized = false;

        /// <summary>
        /// An array storing all resources
        /// </summary>
        [SerializeField]
        protected T[] resources;

        /// <summary>
        /// A dictionary of resources mapped by their Id
        /// </summary>
        [SerializeField]
        protected Dictionary<string, T> resourcesById;
		
        /// <summary>
        /// Gets all of the loaded resources
        /// </summary>
        public T[] Resources {
            get { return resources; }
        }

        /// <summary>
        /// Gets the flag stating if this manager been initialized
        /// </summary>
        protected bool Initialized {
            get { return initialized; }
        }

        /// <summary>
        /// Gets the amount of resources stored
        /// </summary>
        public int Count {
            get { return resources.Length; }
        }

        /// <summary>
        /// Constructs a new resource manager
        /// </summary>
        public ResourceManager() {
            resources = new T[0];
            resourcesById = new Dictionary<string, T>();
        }

        /// <summary>
        /// Gets the given resource by its Id
        /// </summary>
        /// <param name="id">The Id of the resource to get</param>
        /// <returns>Returns the resource that matches the Id, or null if it doesn't exist</returns>
		public T GetById(string id) {
			T resource;
			resourcesById.TryGetValue(id, out resource);
			return resource;
		}

        /// <summary>
        /// Gets the given resource by its Id as the specified type
        /// </summary>
        /// <typeparam name="GetAs">The type to convert the resource to</typeparam>
        /// <param name="id">The Id of the resource to get</param>
        /// <returns>Returns the resource that matches the Id as the specified type, or null if it doesn't exist</returns>
        public GetAs GetById<GetAs>(string id) where GetAs : T {
			return GetById(id) as GetAs;
		}

        /// <summary>
        /// Determines if the given resource Id has been loaded
        /// </summary>
        /// <param name="id">The resource Id to look for</param>
        /// <returns>Returns true if the Id exists, false otherwise</returns>
        public bool Contains(string id) {
            return GetById(id) != null;
        }

        /// <summary>
        /// Determines if the given resource Id of the specified type has been loaded
        /// </summary>
        /// <param name="id">The resource Id to look for</param>
        /// <returns>Returns true if the Id of the given type exists, false otherwise</returns>
        public bool Contains<N>(string id) where N : T {
            return GetById<N>(id) != null;
        }

        #region Loading Functions

        /// <summary>
        /// Loads the specific resources
        /// </summary>
        public abstract void LoadResources();

        /// <summary>
        /// Loads all resources from the given path and processes them
        /// </summary>
        /// <param name="path">The path to load resources from</param>
        /// <param name="clone">Flag indicating if the resources need cloned when loaded</param>
		protected void LoadResources(string path, bool clone = false) {
            // If this manager has not been initialized, then do nothing
            if (initialized) {
                return;
            }

            // Load all of the resources
            T[] loadedResources = UnityEngine.Resources.LoadAll<T>(path);

            resources = loadedResources;
            resourcesById.Clear();
            
            ProcessResources(clone);

            initialized = true;
        }

        /// <summary>
        /// Processes all loaded resources
        /// </summary>
        /// <param name="clone">Flag indicating if the loaded resources should be cloned</param>
        protected void ProcessResources(bool clone) {
            T resource;

            // Clear any already processed data
            resourcesById = new Dictionary<string, T>();

            // Perform any pre-processing actions
            OnPreProcess();

            // Process each resource
            for (int i = 0; i < resources.Length; i++) {
                resource = resources[i];

                // If we want to clone the resource
                if (clone) {
                    // Clone it and overwrite the original
                    resource = UnityEngine.Object.Instantiate(resource);
                    resources[i] = resource;
                }

                // Process the resource
                resourcesById.Add(resource.id, resource);
                OnResourceProcessed(resource);

                // Update the progress of the process operation
                onProgressChanged?.Invoke(false, i / resources.Length, $"Post-processing resource: {resource.id}");
            }

            // Perform any post-processing actions
            OnPostProcess();
        }

        /// <summary>
        /// Method called before any resource processing begins
        /// </summary>
        protected virtual void OnPreProcess() {
        }

        /// <summary>
        /// Method called after all resource processing ends
        /// </summary>
        protected virtual void OnPostProcess() {
        }

        /// <summary>
        /// Method called each time a resource is processed
        /// </summary>
        /// <param name="resource">The resource currently being processed</param>
        protected virtual void OnResourceProcessed(T resource) {
        }

        #endregion
    }
}