using System;
using UnityEngine;

namespace JusticeFramework.Components {
    public delegate void OnAmmoHit(WorldObject hit);

    /// <summary>
    /// Base class for ammo related world object
    /// </summary>
    [Serializable]
    public class Ammo : Item {
        private float LifeTime = 60;

        public event OnAmmoHit onHit;

        [SerializeField]
        private bool sticky;

        [SerializeField]
        private float hitDespawnTime = 15;

        private bool fired;
        
        public override EInteractionType InteractionType {
            get { return EInteractionType.Take; }
        }
        
        public void OnFire() {
            fired = true;
            Invoke("Despawn", LifeTime);
        }

        private void OnCollisionEnter(Collision collision) {
            if (!fired) {
                return;
            }

            WorldObject worldObj = collision.transform.GetComponent<WorldObject>();

            if (sticky) {
                transform.SetParent(collision.transform);

                Rigidbody body = transform.GetComponent<Rigidbody>();
                body.isKinematic = true;
                body.useGravity = false;
            }

            onHit?.Invoke(worldObj);

            CancelInvoke("Despawn");
            Invoke("Despawn", hitDespawnTime);
        }

        private void Despawn() {
            Destroy(gameObject);
        }
    }
}