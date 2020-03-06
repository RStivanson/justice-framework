using JusticeFramework.Core;
using JusticeFramework.Data;
using JusticeFramework.Logic;
using UnityEngine;

namespace JusticeFramework.Interfaces {
    /// <summary>
    /// Base interface used for entity components that are attached to GameObjects
    /// </summary>
    public interface IWorldObject : IDataObject {
		/// <summary>
		/// The transform attached to this object
		/// </summary>
		Transform Transform { get; }
		
		/// <summary>
		/// The name that is displayed in game
		/// </summary>
		string DisplayName { get; }

        /// <summary>
        /// Sets the data model associated with this entity
        /// </summary>
        /// <param name="newModel">The data to attach to this entity</param>
        /// <param name="clone">Flag indicating if the data should be deep cloned</param>
        void SetData(ScriptableDataObject newModel);

        /// <summary>
        /// Gets the assigned data object as the given type.
        /// </summary>
        /// <typeparam name="T">The type of data object to convert to</typeparam>
        /// <returns>Returns the data object in the given type, null if it fails.</returns>
        T GetData<T>() where T : ScriptableDataObject;

        /// <summary>
        /// Activates the world object.
        /// </summary>
        /// <param name="worldObject">The world object that is being activating this object</param>
        /// <returns>Returns an action object that contains extended activation functionality.</returns>
        Action Activate(IWorldObject worldObject);
	}
}
