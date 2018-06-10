using System;
using UnityEngine;

namespace JusticeFramework.Core {
    public delegate void OnHitboxHit(Transform hit);

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
            onHit?.Invoke(other.transform);
        }
    }
}
