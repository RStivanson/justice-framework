using System;
using System.Collections.Generic;
using JusticeFramework.Assets.JusticeFramework.Utility.Extensions;
using JusticeFramework.Components;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.AI {
    /// <summary>
    /// Handles keeping track of, and querying for nearby references
    /// </summary>
	[Serializable]
	public class AiVision : MonoBehaviour {
        /// <summary>
        /// The amount of time between scans
        /// </summary>
		private const float DELAY_BETWEEN_SCANS_IN_SECONDS = 1.5f;

        /// <summary>
        /// The amount of times passed since the last scane
        /// </summary>
		[SerializeField]
		private float timeSinceLastScan;
		
        /// <summary>
        /// Collision layer for picking up entities
        /// </summary>
		[SerializeField]
		private LayerMask scanMask;

        /// <summary>
        /// The distance determining if an entity is in range to be seen
        /// </summary>
		[SerializeField]
		private float scanRadius = 75.0f;

        /// <summary>
        /// The angle in which field of view takes place
        /// </summary>
		[SerializeField]
		private int fieldOfViewAngle = 90;

        /// <summary>
        /// Origin point for all LOS checks
        /// </summary>
        [SerializeField]
        private Vector3 losOrigin;

        /// <summary>
        /// Cached reference to this transform
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private Transform myTransform;
		
        /// <summary>
        /// A list of references last determined as nearby
        /// </summary>
		private List<WorldObject> nearbyWorldObjects;

        /// <summary>
        /// The nearby references processed by reference type
        /// </summary>
		private Dictionary<Type, List<WorldObject>> referencesByType;
		
        /// <summary>
        /// Gets the time since last scan
        /// </summary>
		public float TimeSinceLastScan {
			get { return timeSinceLastScan; }
		}
		
        /// <summary>
        /// Initializes the script
        /// </summary>
		private void Awake() {
			myTransform = transform;
			timeSinceLastScan = 0;

			nearbyWorldObjects = new List<WorldObject>();
            referencesByType = new Dictionary<Type, List<WorldObject>>();
		}
 
        /// <summary>
        /// Updates the script after the update methods have finished, called every frame.
        /// </summary>
		private void LateUpdate() {
			timeSinceLastScan += Time.deltaTime;

            // If the time exceeds the delay, process the surroundings
			if (timeSinceLastScan >= DELAY_BETWEEN_SCANS_IN_SECONDS) {
			    ScanSurroundings();
			    timeSinceLastScan = 0;
			}
		}
		
        /// <summary>
        /// Draws gizmos to show the current setting in the editor
        /// </summary>
		private void OnDrawGizmosSelected() {
			// Draw detection radius
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, scanRadius);

			/*
			Vector3 fovLeft = Quaternion.AngleAxis(-halfFieldOfViewAngle, transform.up) * transform.forward * scanRadius;
			Vector3 fovRight = Quaternion.AngleAxis(halfFieldOfViewAngle, transform.up) * transform.forward * scanRadius;
			
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(transform.position, fovLeft);
			Gizmos.DrawRay(transform.position, fovRight);
			*/
		}

        /// <summary>
        /// Scans the surroundings and processes found entities
        /// </summary>
		private void ScanSurroundings() {
            // Clear the current data
			nearbyWorldObjects.Clear();
			referencesByType.Clear();
			
            // Find all colliders within the scan radius
			Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, scanRadius);

            // Process each collider
			foreach (Collider nearbyCollider in nearbyColliders) {
                // Get the objects reference script
				WorldObject reference = nearbyCollider.transform.GetComponentInCurrentOrParent<WorldObject>();

                // If the script is null or us, skip
				if (reference == null || reference.transform == myTransform) {
					continue;
				}

                // Add the script to the nearby reference pool
				nearbyWorldObjects.Add(reference);

                // Sort the reference by its type
                referencesByType.AddToList(reference.GetType(), reference);
			}
		}
		
		public T NearestQuery<T>() where T : WorldObject {
			return NearestQuery<T>(null, float.MaxValue);
		}
		
		public T NearestQuery<T>(float withinDistance) where T : WorldObject {
			return NearestQuery<T>(null, withinDistance);
		}

		public T NearestQuery<T>(Predicate<T> matchCondition, float withinDistance) where T : WorldObject {
			T closest = null;
			float lastSqrDistance = float.MaxValue;
			
			// Search dictionary of type sorted references for the closest one
			List<WorldObject> referenceList;
			referencesByType.TryGetValue(typeof(T), out referenceList);

			// If nothing is nearby of that type, return nothing
			if (referenceList == null) {
				return null;
			}

			foreach (WorldObject reference in referenceList) {
				if (matchCondition != null && !matchCondition((T)reference)) {
					continue;
				}

				float currentSqrDistance = (myTransform.position - reference.transform.position).sqrMagnitude;

				if (currentSqrDistance >= lastSqrDistance) {
					continue;
				}

				if (currentSqrDistance >= withinDistance) {
					continue;
				}
				
				closest = (T)reference;
				lastSqrDistance = currentSqrDistance;
			}

			return closest;
		}

		public T[] NearbyQuery<T>(bool sortedByClosest = false) where T : WorldObject {
			return NearbyQuery<T>(null, float.MaxValue, sortedByClosest);
		}
		
		public T[] NearbyQuery<T>(float withinDistance, bool sortedByClosest = false) where T : WorldObject {
			return NearbyQuery<T>(null, withinDistance, sortedByClosest);
		}
		
		public T[] NearbyQuery<T>(Predicate<T> matchCondition, float withinDistance, bool sortedByClosest = false) where T : WorldObject {
			List<T> nearbyList = new List<T>();
			
			// Search dictionary of type sorted references for the closest one
			List<WorldObject> referenceList;
			referencesByType.TryGetValue(typeof(T), out referenceList);

			// If nothing is nearby of that type, return nothing
			if (referenceList == null) {
				return null;
			}

			foreach (WorldObject reference in referenceList) {
				if (matchCondition != null && !matchCondition((T)reference)) {
					continue;
				}

				float currentSqrDistance = (myTransform.position - reference.transform.position).sqrMagnitude;

				if (currentSqrDistance >= withinDistance) {
					continue;
				}
				
				nearbyList.Add((T)reference);
			}

			if (sortedByClosest) {
				nearbyList.Sort(CompareWorldObjectDistance);
			}

			return nearbyList.ToArray();
		}

		private int CompareWorldObjectDistance(WorldObject left, WorldObject right) {
			float leftDistance = (left.Transform.position - myTransform.position).sqrMagnitude;
			float rightDistance = (right.Transform.position - myTransform.position).sqrMagnitude;

			if (leftDistance < rightDistance) {
				return -1;
			}
			
			return leftDistance > rightDistance ? 1 : 0;
		}
	}
}
