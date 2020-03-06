using UnityEngine;

namespace JusticeFramework.Core.Extensions {
	/// <summary>
	/// A collection of extension methods that extend the functionality of Unity transforms
	/// </summary>
	public static class TransformExtensions {
		/// <summary>
		/// Attempts to retrieve a component in the transform or its parent
		/// </summary>
		/// <param name="transform">The transform to check against</param>
		/// <typeparam name="T">The type of component to get</typeparam>
		/// <returns>Returns the instance of the component on this transform or its parent, else null if not found</returns>
		public static T GetComponentInCurrentOrParent<T>(this Transform transform) {
			T component = transform.GetComponent<T>();

			// If we failed to get the component from the current object, check its parent
			if (component == null) {
				component = transform.GetComponentInParent<T>();
			}

			return component;
		}

		/// <summary>
		/// Performs a raycast from the center of the transform for the given distance and returns its hit data
		/// </summary>
		/// <param name="transform">The transform to raycast from</param>
		/// <param name="raycastHit">The data returned from the raycast</param>
		/// <param name="distance">The distance that the raycast should go</param>
		/// <returns>Returns true if something was hit by the raycast, false otherwise</returns>
		public static bool RaycastFromCenterForward(this Transform transform, out RaycastHit raycastHit, float distance){
			return Physics.Raycast(transform.position, transform.forward, out raycastHit, distance);
		}

		/// <summary>
		/// Destroys all children attached to the transform
		/// </summary>
		/// <param name="transform">The transform whose children should be destroyed</param>
		public static void DestroyAllChildren(this Transform transform, int startingIndex = 0) {
			// For each child attached to this transform, destroy it
            for (int i = startingIndex; i < transform.childCount; i++) {
				Object.Destroy(transform.GetChild(i).gameObject);
			}
		}
	}
}
