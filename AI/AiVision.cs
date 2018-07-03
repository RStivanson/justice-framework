using System;
using System.Collections.Generic;
using JusticeFramework.Utility.Extensions;
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
		
        /// <summary>
        /// Queries for the closest entity of the given type
        /// </summary>
        /// <typeparam name="T">The type to look for</typeparam>
        /// <returns>Returns the closest entity, or null if none exist</returns>
		public T NearestQuery<T>() where T : WorldObject {
			return NearestQuery<T>(null, float.MaxValue);
		}

        /// <summary>
        /// Queries for the closest entity of the given type within the given distance
        /// </summary>
        /// <typeparam name="T">The type to look for</typeparam>
        /// <param name="withinDistance">The minimum distance the entities must be in</param>
        /// <returns>Returns the closest entity, or null if none exist</returns>
        public T NearestQuery<T>(float withinDistance) where T : WorldObject {
			return NearestQuery<T>(null, withinDistance);
		}

        /// <summary>
        /// Queries for the closest entity of the given type within the given distance that matches the given condition.
        /// </summary>
        /// <typeparam name="T">The type to look for</typeparam>
        /// <param name="matchCondition">A custom condition to match the entitie against. A null value will skip the condition.</param>
        /// <param name="withinDistance">The minimum distance the entities must be in</param>
        /// <returns>Returns the closest entity, or null if none exist</returns>
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

            // For each nearby reference
			foreach (WorldObject reference in referenceList) {
                // Checks the match condition
                bool matchedCondition = matchCondition?.Invoke((T)reference) ?? true;

                // Calculate the distance
				float currentSqrDistance = (myTransform.position - reference.transform.position).sqrMagnitude;

                // If the reference didnt match the condition, not the closest, or not within the distance then do nothing
				if (!matchedCondition || currentSqrDistance >= lastSqrDistance || currentSqrDistance >= withinDistance) {
					continue;
				}

                // Set the new found reference
				closest = (T)reference;
				lastSqrDistance = currentSqrDistance;
			}

			return closest;
		}

        /// <summary>
        /// Gets a list of nearby references
        /// </summary>
        /// <typeparam name="T">The type of reference to look for</typeparam>
        /// <param name="sortedByClosest">Flag indicating if the list should be sorted by distance</param>
        /// <returns>Returns a list of nearby references, or an empty array if none exist</returns>
		public T[] NearbyQuery<T>(bool sortedByClosest = false) where T : WorldObject {
			return NearbyQuery<T>(null, -1, sortedByClosest);
		}

        /// <summary>
        /// Gets a list of nearby references within the given distance
        /// </summary>
        /// <typeparam name="T">The type of reference to look for</typeparam>
        /// <param name="withinDistance">The minimum distance the entities must be in</param>
        /// <param name="sortedByClosest">Flag indicating if the list should be sorted by distance</param>
        /// <returns>Returns a list of nearby references within the distance, or an empty array if none exist</returns>
        public T[] NearbyQuery<T>(float withinDistance, bool sortedByClosest = false) where T : WorldObject {
			return NearbyQuery<T>(null, withinDistance, sortedByClosest);
		}

        /// <summary>
        /// Gets a list of nearby references within the given distance
        /// </summary>
        /// <typeparam name="T">The type of reference to look for</typeparam>
        /// <param name="matchCondition">A custom condition to match the entitie against. A null value will skip the condition.</param>
        /// <param name="withinDistance">The minimum distance the entities must be in</param>
        /// <param name="sortedByClosest">Flag indicating if the list should be sorted by distance</param>
        /// <returns>Returns a list of nearby references within the distance and matches the condition, or an empty array if none exist</returns>
        public T[] NearbyQuery<T>(Predicate<T> matchCondition, float withinDistance, bool sortedByClosest = false) where T : WorldObject {
			List<T> nearbyList = new List<T>();
			
			// Search dictionary of type sorted references for the closest one
			List<WorldObject> referenceList;
			referencesByType.TryGetValue(typeof(T), out referenceList);

			// If nothing is nearby of that type, return nothing
			if (referenceList == null) {
				return null;
			}

            // For each nearby reference
            foreach (WorldObject reference in referenceList) {
                // Checks the match condition
                bool matchedCondition = matchCondition?.Invoke((T)reference) ?? true;

                // If the distance is valid
                if (withinDistance != -1) {
                    // Calculate the distance
                    float currentSqrDistance = (myTransform.position - reference.transform.position).sqrMagnitude;

                    // If the distance is within the allotted distance
                    if (currentSqrDistance >= withinDistance) {
                        continue;
                    }
                }
				
                // Add the reference to the list
				nearbyList.Add((T)reference);
			}

            // If the list should be sorted
			if (sortedByClosest) {
                // Sort the list
				nearbyList.Sort(CompareWorldObjectDistance);
			}

			return nearbyList.ToArray();
		}

        /// <summary>
        /// Compares two WorldObjects and determines the closest, used for sorting.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>Returns -1 if the left object is closest, 1 if the right is the closest, else 0</returns>
		private int CompareWorldObjectDistance(WorldObject left, WorldObject right) {
            // Calculate the distances
			float leftDistance = (left.Transform.position - myTransform.position).sqrMagnitude;
			float rightDistance = (right.Transform.position - myTransform.position).sqrMagnitude;

            // If the left is closest, return -1
			if (leftDistance < rightDistance) {
				return -1;
			}
			
            // Return 1 is right is closest, else 0
			return leftDistance > rightDistance ? 1 : 0;
		}
	}
}
