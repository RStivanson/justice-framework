using UnityEngine;

namespace JusticeFramework.Utility.Extensions {
	public static class TransformExtensions {
		public static T GetComponentInCurrentOrParent<T>(this Transform transform) {
			T component = default(T);
			
			component = transform.GetComponent<T>();

			if (component == null) {
				component = transform.GetComponentInParent<T>();
			}

			return component;
		}

		public static bool RaycastFromCenterForward(this Transform transform, out RaycastHit raycastHit, float distance){
			return Physics.Raycast(transform.position, transform.forward, out raycastHit, distance);
		}

		public static void DestroyAllChildren(this Transform transform) {
			foreach (Transform child in transform) {
				Object.Destroy(child.gameObject);
			}
		}
	}
}
