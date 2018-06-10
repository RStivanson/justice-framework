using UnityEngine;

namespace JusticeFramework.Core.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for armor
	/// </summary>
	public interface IArmor : IEquippable {
		/// <summary>
		/// The defense rating of this armor piece
		/// </summary>
		int ArmorRating { get; }

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