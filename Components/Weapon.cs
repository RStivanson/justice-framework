using System;
using JusticeFramework.Core.Models;
using JusticeFramework.Core;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
using UnityEngine;
using JusticeFramework.Core.Managers;

namespace JusticeFramework.Components {
	[Serializable]
	public class Weapon : Item, IWeapon {
		public event OnItemEquipped OnItemEquipped;
		public event OnItemUnequipped OnItemUnequipped;

#region Variables

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

#endregion

#region Properties

		private WeaponModel WeaponModel {
			get { return model as WeaponModel; }
		}
		
		public EEquipSlot EquipSlot {
			get { return WeaponModel.equipSlot; }
		}

		public AudioClip EquipSound {
			get { return WeaponModel.equipSound; }
		}

		public EWeaponType WeaponType {
			get { return WeaponModel.weaponType; }
		}
		
		public int Damage {
			get { return WeaponModel.damage; }
        }

        public EWeaponFireType FireType {
            get { return WeaponModel.fireType; }
        }

        public EAmmoType AcceptedAmmo {
            get { return WeaponModel.acceptedAmmo; }
        }

        public AnimationClip AttackAnimation {
            get { return WeaponModel.attackAnimation; }
        }

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

        #endregion

        protected override void OnIntialized() {
            base.OnIntialized();

            if (FireType == EWeaponFireType.Hitbox)
                hitbox.HitCollider.enabled = false;
        }

        public void SetOwner(WorldObject actor) {
            owner = actor as Actor;
        }

        public bool CanFire() {
            return lastAttackTime == -1 || (Time.time - lastAttackTime) > AttackAnimation.length;
        }

        public void StartFire(IContainer ammoSupply = null) {
            switch (FireType) {
                case EWeaponFireType.Projectile:
                    if (ammoSupply.TakeItem("TestArrow", 1)) {
                        AmmoModel ammo = GameManager.AssetManager.GetById<AmmoModel>("TestArrow");
                        GameObject go = Instantiate(ammo.prefab);

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

        public void UpdateFire(IContainer ammoSupply = null) {

        }

        public void EndFire(Transform origin, IContainer ammoSupply = null) {
            if (!CanFire()) {
                return;
            }

            lastAttackTime = Time.time;

            switch (FireType) {
                case EWeaponFireType.Linear:
                    RaycastHit hit;
                    
                    if (Physics.Raycast(origin.position, origin.forward, out hit, 10)) {
                        Actor actor = hit.transform.GetComponent<Actor>();

                        if (actor != null) {
                            actor.Damage(owner, Damage);
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
                actor.Damage(owner, Damage);
            }
        }
    }
}