using UnityEngine;

namespace JusticeFramework.AI {
	/// <summary>
	/// Steering behaviour implementation used for life like pathing behaviours
	/// </summary>
	public static class Steering {
		/// <summary>
		/// Calculates a steering velocity that seeks the given target
		/// </summary>
		/// <param name="start">The position of the object wishing to seek</param>
		/// <param name="target">The position to seek</param>
		/// <param name="currentVelocity">The current velocity of the object seeking</param>
		/// <param name="maxVelocity">The max velocity allowed</param>
		/// <returns>Returns a velocity adjustment that seeks the target</returns>
		public static Vector3 Seek(Vector3 start, Vector3 target, Vector3 currentVelocity, float maxVelocity) {
			Vector3 desiredVelocity = (target - start).normalized * maxVelocity;
			return desiredVelocity - currentVelocity;
		}
		
		/// <summary>
		/// Calculates a steering velocity that flees from the given target
		/// </summary>
		/// <param name="start">The position of the object wishing to flee</param>
		/// <param name="target">The position to flee from</param>
		/// <param name="currentVelocity">The current velocity of the object fleeing</param>
		/// <param name="maxVelocity">The max velocity allowed</param>
		/// <returns>Returns a velocity adjustment that flees from the target</returns>
		public static Vector3 Flee(Vector3 start, Vector3 target, Vector3 currentVelocity, float maxVelocity) {
			Vector3 desiredVelocity = (start - target).normalized * maxVelocity;
			return desiredVelocity - currentVelocity;
		}

		/// <summary>
		/// Calculates a steering velocity that arrives at a given target
		/// </summary>
		/// <param name="start">The position of the object wishing to arrive</param>
		/// <param name="target">The position to arrive at</param>
		/// <param name="currentVelocity">The current velocity of the object arriving</param>
		/// <param name="maxVelocity">The max velocity allowed</param>
		/// <param name="sqrSlowingRadius">The square distance that defines the slowing radius</param>
		/// <returns>Returns a velocity adjustment that arrives at the target</returns>
		public static Vector3 Arrive(Vector3 start, Vector3 target, Vector3 currentVelocity, float maxVelocity, float sqrSlowingRadius) {
			Vector3 desiredVelocity = target - start;
			float sqrDistance = desiredVelocity.sqrMagnitude;
			
			// Scale the desired velocity to match the maximum velocity scalar
			desiredVelocity = desiredVelocity.normalized * maxVelocity;
			
			// If we are inside the slowing radius, scale the velocity down based on the distance
			if (sqrDistance <= sqrSlowingRadius) {
				desiredVelocity *= sqrDistance / sqrSlowingRadius;
			}
			
			return desiredVelocity - currentVelocity;
		}

		public static Vector3 Wander(Vector3 start, Vector3 currentVelocity, Vector3 up, float circleDistance, float wanderAngle = 45, float circleRadius = 1) {
			Vector3 circleCenter = currentVelocity.normalized * circleDistance;
			Vector3 displacement = new Vector3(0, 0, 1) * circleRadius;
			
			// Rotate the displacement vector to the wander angle
			displacement = Quaternion.AngleAxis(wanderAngle, up) * displacement;
			
			// Add the resulting vectors to get the wander force
			return circleCenter + (displacement);
		}

		public static Vector3 Pursue(Vector3 start, Vector3 target, Vector3 currentVelocity, Vector3 targetVelocity, float maxVelocity) {
			Vector3 futureTargetPosition = target - start;
			float prediction = futureTargetPosition.sqrMagnitude / maxVelocity;
			futureTargetPosition = target + targetVelocity * prediction;
			return Seek(start, futureTargetPosition, currentVelocity, maxVelocity);
		}
		
		public static Vector3 Evade(Vector3 start, Vector3 target, Vector3 currentVelocity, Vector3 targetVelocity, float maxVelocity) {
			Vector3 futureTargetPosition = target - start;
			float prediction = futureTargetPosition.sqrMagnitude / maxVelocity;
			futureTargetPosition = target + targetVelocity * prediction;
			return Flee(start, futureTargetPosition, currentVelocity, maxVelocity);
		}
	}
}