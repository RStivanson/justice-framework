using JusticeFramework.Components;
using System;
using UnityEngine;

namespace JusticeFramework {
    public delegate void OnHitboxHit(WorldObject worldObject);

    [Serializable]
    [RequireComponent(typeof(Collider))]
    public class Hitbox : MonoBehaviour {
        public event OnHitboxHit onHit;

        [SerializeField]
        private Collider hitCollider;

        public Collider HitCollider {
            get { return hitCollider; }
        }

        private void OnValidate() {
            if (hitCollider == null) {
                hitCollider = GetComponent<Collider>();
            }
        }

        private void OnTriggerEnter(Collider other) {
            WorldObject worldObject = other.transform.GetComponent<WorldObject>();

            if (worldObject != null) {
                onHit?.Invoke(worldObject);
            }
        }
    }
}
