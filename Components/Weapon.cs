using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Managers;
using System;
using UnityEngine;

namespace JusticeFramework.Components {
    [Serializable]
	public class Weapon : Item {
        [SerializeField]
        private Renderer thisRenderer;

		[SerializeField]
		private Rigidbody thisRigidbody;

		[SerializeField]
		private BoxCollider thisCollider;

        [SerializeField]
        private Transform offhandIkTarget;

        [SerializeField]
        private Hitbox hitbox;

        private Ammo loadedProjectile;

        private Actor owner;

        private float lastAttackTime = -1;

        public Renderer Renderer {
            get { return thisRenderer; }
        }

        public Rigidbody Rigidbody {
            get { return thisRigidbody; }
        }

        public Collider Collider {
            get { return thisCollider; }
        }

        public Transform OffhandIkTarget {
            get { return offhandIkTarget; }
        }

        protected override void OnIntialized() {
            base.OnIntialized();

            if (GetData<WeaponData>().FireType == EWeaponFireType.Hitbox) {
                hitbox.HitCollider.enabled = false;
                hitbox.onHit += OnHit;
            }
        }

        public bool CanFire() {
            return lastAttackTime == -1 || (Time.time - lastAttackTime) > 1;
        }

        public void StartFire(IContainer ammoSupply = null) {
            WeaponData data = dataObject as WeaponData;

            switch (data.FireType) {
                case EWeaponFireType.Hitbox:
                    hitbox.enabled = true;

                    break;
                case EWeaponFireType.Projectile:
                    string testArrowId = "TestArrow";

                    if (ammoSupply.Inventory.Contains(testArrowId, 1)) {
                        ammoSupply.Inventory.Remove(testArrowId, 1);

                        AmmoData ammo = GameManager.DataManager.GetAssetById<AmmoData>("TestArrow");
                        GameObject go = Instantiate(ammo.ScenePrefab);

                        loadedProjectile = go.GetComponent<Ammo>();

                        if (loadedProjectile == null) {
                            Destroy(go);
                        } else {
                            loadedProjectile.transform.SetParent(transform);
                            loadedProjectile.transform.localPosition = Vector3.zero;
                            loadedProjectile.transform.rotation = transform.rotation;

                            Rigidbody projectileRigidbody = loadedProjectile.GetComponent<Rigidbody>();
                            projectileRigidbody.isKinematic = true;
                            projectileRigidbody.useGravity = false;
                        }
                    }

                    break;
            }
        }

        public EAttackStatus UpdateFire(IContainer ammoSupply = null) {
            return EAttackStatus.Empty;
        }

        public void EndFire(Transform origin, IContainer ammoSupply = null) {
            if (!CanFire()) {
                return;
            }

            lastAttackTime = Time.time;
            WeaponData data = dataObject as WeaponData;

            switch (data.FireType) {
                case EWeaponFireType.Hitbox:
                    hitbox.enabled = false;

                    break;
                case EWeaponFireType.Linear:
                    RaycastHit hit;
                    
                    if (Physics.Raycast(origin.position, origin.forward, out hit, 10)) {
                        Actor actor = hit.transform.GetComponent<Actor>();

                        if (actor != null) {
                            actor.Damage(owner, data.Damage);
                        }
                    }

                    break;
                case EWeaponFireType.Projectile:
                    if (loadedProjectile != null) {
                        FireProjectile(origin, loadedProjectile);
                        loadedProjectile = null;
                    }
                    
                    break;
            }
        }

        private void FireProjectile(Transform origin, Ammo projectile) {
            projectile.transform.SetParent(null);

            projectile.OnFire();
            projectile.onHit += OnHit;

            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.useGravity = true;
            projectileRigidbody.AddForce(origin.forward * 25, ForceMode.Impulse);
        }

        private void OnHit(WorldObject hit) {
            Actor actor = hit as Actor;

            if (actor != null) {
                actor.Damage(owner, GetData<WeaponData>().Damage);
            }
        }
    }
}