using UnityEngine;

namespace JusticeFramework.Interfaces {
    /// <summary>
    /// Interface that defines methods used by game objects that should have their rigs overriden
    /// </summary>
    public interface IRigged {
        /// <summary>
        /// Sets the bones of the object to the given renderer's bones
        /// </summary>
        /// <param name="renderer">The renderer to set the bones to</param>
        void SetBones(SkinnedMeshRenderer renderer);

        /// <summary>
        /// Resets the objects bones to its original bones
        /// </summary>
        void ClearBones();
    }
}
