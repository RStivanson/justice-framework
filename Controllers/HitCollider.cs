using UnityEngine;

namespace JusticeFramework.Controllers {
	public delegate void OnHitColliderHit(Collider hit);

	public class HitCollider : MonoBehaviour {
		public OnHitColliderHit onHitColliderHit;

		public void OnTriggerEnter(Collider other) {
			Debug.Log("\"" + other.name + "\" has been hit!");
			onHitColliderHit?.Invoke(other);
		}
	}
}