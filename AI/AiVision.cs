using System;
using System.Collections.Generic;
using JusticeFramework.Components;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.AI {
	[Serializable]
	public class AiVision : MonoBehaviour {
		private const float DELAY_BETWEEN_SCANS_IN_SECONDS = 1.5f;

		[SerializeField]
		private Transform myTransform;
		
		[SerializeField]
		private float timeSinceLastScan;
		
		[SerializeField]
		private LayerMask scanMask;

		[SerializeField]
		private float scanRadius = 75.0f;

		[SerializeField]
		private int fieldOfViewAngle = 90;
		
		private List<WorldObject> nearbyReferences;
		private Dictionary<Type, List<WorldObject>> referencesByType;
		
		public float TimeSinceLastScan {
			get { return timeSinceLastScan; }
		}
		
		private void Awake() {
			myTransform = transform;
			timeSinceLastScan = 0;

			nearbyReferences = new List<WorldObject>();
			referencesByType = new Dictionary<Type, List<WorldObject>>();
			
			ScanSurroundings();
		}
 
		private void Update() {
			timeSinceLastScan += Time.deltaTime;

			if (timeSinceLastScan < DELAY_BETWEEN_SCANS_IN_SECONDS) {
				return;
			}

			ScanSurroundings();
			timeSinceLastScan = 0;
		}
		
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

		private void ScanSurroundings() {
			nearbyReferences.Clear();
			referencesByType.Clear();
			
			Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, scanRadius);

			foreach (Collider nearbyCollider in nearbyColliders) {
				WorldObject reference = nearbyCollider.transform.GetComponentInCurrentOrParent<WorldObject>();

				if (reference == null || reference.transform == myTransform) {
					continue;
				}

				nearbyReferences.Add(reference);

				List<WorldObject> referenceList;
				referencesByType.TryGetValue(reference.GetType(), out referenceList);
					
				if (referenceList == null) {
					referenceList = new List<WorldObject>();
					referencesByType.Add(reference.GetType(), referenceList);
				}
					
				referenceList.Add(reference);
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
				nearbyList.Sort(CompareReferenceDistance);
			}

			return nearbyList.ToArray();
		}

		private int CompareReferenceDistance(WorldObject left, WorldObject right) {
			float leftDistance = (left.Transform.position - myTransform.position).sqrMagnitude;
			float rightDistance = (right.Transform.position - myTransform.position).sqrMagnitude;

			if (leftDistance < rightDistance) {
				return -1;
			}
			
			return leftDistance > rightDistance ? 1 : 0;
		}
	}
}
