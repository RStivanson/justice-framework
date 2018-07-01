using System;
using System.Collections.Generic;
using JusticeFramework.AI;
using JusticeFramework.Console;
using JusticeFramework.Core.Models;
using JusticeFramework.Core.Collections;
using JusticeFramework.Core;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Interfaces;
using JusticeFramework.UI.Views;
using JusticeFramework.Utility.Extensions;
using UnityEngine;
using JusticeFramework.Core.StatusEffects;
using JusticeFramework.StatusEffects;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Console;

namespace JusticeFramework.Components {
	public delegate void OnEnterCombat(Actor actorNowInCombat);
	public delegate void OnExitCombat(Actor actorNoLongerInCombat);
	public delegate void OnLevelUp(Actor leveledUp, int newLevel);

	/// <summary>
	/// This class houses all model and functions relating for actors (NPCs and Player)
	/// </summary>
	[Serializable]
	public class Actor : WorldObject, IActor, IDamageable, IInventory {
		public const int HEROIC_ATTACK_BUFFER = 3;
		
		public event OnCurrentHealthChanged onCurrentHealthChanged;
		public event OnDamageTaken onDamageTaken;
		public event OnHealingRecieved onHealingRecieved;
		public event OnDeath onDeath;
		public event OnItemAdded onItemAdded;
		public event OnItemRemoved onItemRemoved;
		public event OnLevelUp onLevelUp;

		public event OnEnterCombat onEnterCombat;
		public event OnExitCombat onExitCombat;

#region Variables
		
		/// <summary>
		/// The attached animator component
		/// </summary>
		[SerializeField]
		private Animator animator;

        private EntityAnimator entityAnimator;
		
		/// <summary>
		/// Equipment array storing all equipped items
		/// </summary>
		[SerializeField]
		private IEquippable[] equipment;

        /// <summary>
        /// SkinnedMeshRenderer attached to the Body mesh
        /// </summary>
        [SerializeField]
        public SkinnedMeshRenderer meshRenderer;
        
        /// <summary>
        /// The current health value of this actor
        /// </summary>
        [SerializeField]
        private float currentHealth = 100;

        /// <summary>
        /// The amount of experience held by the actor
        /// </summary>
        [SerializeField]
        private int currentExperience = 0;
		
		/// <summary>
		/// Flag stating if this actor is currently in combat
		/// </summary>
		[SerializeField]
		private bool isInCombat;

		/// <summary>
		/// A list of all NPCs currently attacking this target
		/// </summary>
		[SerializeField]
		private List<IActor> threats;

        [SerializeField]
        private AnimationClip defaultAttackAnimation;

        [SerializeField]
        private AnimationClip replaceableAnimationClip;

		/// <summary>
		/// The rigidbody components attached to all pieces of the GameObject
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private Rigidbody[] ragdollRigidbodies;

        [SerializeField]
        private Transform mainhandBone;

        [SerializeField]
        private Transform offhandBone;

        /// <summary>
        /// Game events to take place when this character dies
        /// </summary>
        [SerializeField]
        private GameEvent[] onKill;

        /// <summary>
        /// List of status effects currently applied to this actor
        /// </summary>
        [SerializeField]
        private List<StatusEffect> statusEffects;

#endregion

#region Properties
		
		private ActorModel ActorModel {
			get { return model as ActorModel; }
		}
		
		/// <inheritdoc />
		public override bool CanBeActivated {
			get { return (ActorModel?.canBeActivated ?? false) && !isInCombat; }
		}
		
		public override EInteractionType InteractionType {
			get { return IsDead ? EInteractionType.Loot : EInteractionType.Talk; }
		}

		public Inventory Inventory {
			get { return ActorModel.inventory; }
		}
		
		/// <summary>
		/// The base health of this actor
		/// </summary>
		public int BaseHealth {
			get { return ActorModel.baseHealth; }
		}
		
		/// <summary>
		/// The maximum health of this actor
		/// </summary>
		public int MaxHealth {
			get { return ActorModel.baseHealth + (Level * 25); }
		}
		
		/// <summary>
		/// The current health of this actor
		/// </summary>
		public float CurrentHealth {
			get { return currentHealth; }
			set {
				// If the health values are roughly the same thing, do nothing
				/*if (Math.Abs(ActivatorModel.currentHealth - value) < 0.01f) {
					return;
				}*/
				
				currentHealth = value;
				onCurrentHealthChanged?.Invoke(this);
			}
		}
		
		public int Level {
			get { return 1 + (CurrentExperience / 450); }
		}

		public int CurrentExperience {
			get { return currentExperience; }
			set {
				if (currentExperience == value) {
					return;
				}

				int currentLevel = Level;
				currentExperience = value;
				int newLevel = Level;

				if (currentLevel != newLevel) {
					onLevelUp?.Invoke(this, newLevel);
				}
			}
		}

		/// <summary>
		/// Flag stating if this actor is invincible (is killable)
		/// </summary>
		public bool IsInvincible {
			get { return ActorModel.isInvincible; }
			set { ActorModel.isInvincible = value; }
		}
		
		/// <summary>
		/// Flag stating if this actor is currently dead
		/// </summary>
		public bool IsDead {
			get { return currentHealth <= 0 && !ActorModel.isInvincible; }
		}
		
		public EBattleConfidence BattleConfidence {
			get { return ActorModel.battleConfidence; }
			set { ActorModel.battleConfidence = value; }
		}

		public EAggression Aggression {
			get { return ActorModel.aggression; }
			set { ActorModel.aggression = value; }
		}

		public EMorals Morals {
			get { return ActorModel.morals; }
			set { ActorModel.morals = value; }
		}

		public EInterestOrigin InterestOrigin {
			get { return ActorModel.interestOrigin; }
			set { ActorModel.interestOrigin = value; }
		}
		
		public float LoseInterestDistance {
			get { return ActorModel.loseInterestDistance; }
			set { ActorModel.loseInterestDistance = value; }
		}
		
		/// <summary>
		/// Flag stating if this actor is in combat
		/// </summary>
		public bool IsInCombat {
			get { return isInCombat; }
		}

		public List<IActor> Threats {
			get { return threats; }
		}

		/// <summary>
		/// The total armor rating of all equipped items
		/// </summary>
		public int TotalArmor {
			get {
				int totalArmorRating = 0;
				Armor temp;

				for (int i = 0; i < (int)EEquipSlot.Waist; ++i) {
					temp = equipment[i] as Armor;

					if (temp != null) {
						totalArmorRating += temp.ArmorRating;
					}
				}

				return totalArmorRating;
			}
		}
		
		/// <summary>
		/// The percentage of reduced damage taken when dealt damage
		/// </summary>
		public float DamageReductionPercent {
			get { return (TotalArmor * 0.08f) / 100; }
		}

		/// <summary>
		/// The total amount of damage output by this actor
		/// </summary>
		public int TotalDamage {
			get {
				Weapon weapon = equipment[(int)EEquipSlot.Mainhand] as Weapon;
				return weapon != null ? weapon.Damage : 5;
			}
		}

#endregion

		/// <inheritdoc cref="WorldObject" />
		protected override void OnIntialized() {
            entityAnimator = new EntityAnimator(animator);
			ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            statusEffects = new List<StatusEffect>();

			threats = new List<IActor>();
			equipment = new IEquippable[Enum.GetNames(typeof(EEquipSlot)).Length];
			
            /*
			Equip(GameManager.Spawn("TestHelm001") as IEquippable);
			Equip(GameManager.Spawn("TestChestplate001") as IEquippable);
			Equip(GameManager.Spawn("TestPlatelegs001") as IEquippable);
			Equip(GameManager.Spawn("TestSword001") as IEquippable);
            */

			ExitCombat();
		}

        protected virtual void Update() {
            ProcessStatusEffects(Time.deltaTime);
        }

		private void OnDrawGizmosSelected() {
            if (ActorModel == null) return;

			// Draw detection radius
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, LoseInterestDistance);
		}

#region Health Functions
		
		[ConsoleCommand("kill", "Kills the target actor unless it is invincible", ECommandTarget.LookAt)]
		public void Kill() {
			Damage(null, currentHealth, true);
		}
		
        [ConsoleCommand("dam", "Damages the yourself")]
		public void Damage(Actor attacker, float damageAmount, bool ignoreDamageReduction = false) {
			if (attacker != null) {
				if (!isInCombat) {
					EnterCombat(attacker);
				} else {
					if (!threats.Contains(attacker)) {
						threats.Add(attacker);
					}
				}
			}

			if (!ignoreDamageReduction) {
				damageAmount -= damageAmount * DamageReductionPercent;
			}

			currentHealth -= damageAmount;

			if (currentHealth <= 0) {
				if (ActorModel.isInvincible) {
					currentHealth = 1;
				} else {
					currentHealth = 0;
					onDeath?.Invoke(this, attacker);

                    foreach (GameEvent killEvent in onKill) {
                        killEvent.Execute(this, attacker);
                    }

					ExitCombat();
				}
			}

			if (currentHealth > MaxHealth) {
				currentHealth = MaxHealth;
			}

			OnReferenceStateChanged();
			onDamageTaken?.Invoke(this, attacker, damageAmount);
			onCurrentHealthChanged?.Invoke(this);
		}

		public void Heal(Actor healer, float amount) {
			currentHealth += amount;

			if (currentHealth <= 0) {
				if (ActorModel.isInvincible) {
					currentHealth = 1;
				} else {
					currentHealth = 0;

					ExitCombat();
				}
			}

			if (currentHealth > MaxHealth) {
				currentHealth = MaxHealth;
			}

			OnReferenceStateChanged();
			onHealingRecieved?.Invoke(this, healer, amount);
			onCurrentHealthChanged?.Invoke(this);
		}
		
#endregion
		
#region Experience Functions

		[ConsoleCommand("addexp", "Adds experience to the target actor", ECommandTarget.LookAt)]
		public void AddExperience(int amount) {
			CurrentExperience = CurrentExperience + amount;
		}
		
		[ConsoleCommand("removeexp", "Removes experience from the target actor", ECommandTarget.LookAt)]
		public void RemoveExperience(int amount) {
			CurrentExperience = CurrentExperience - amount;
		}
		
#endregion

#region Combat
		
		public void EnterCombat(IActor firstEnemy = null) {
			isInCombat = true;
			threats.Clear();
			
			if (firstEnemy != null) {
				threats.Add(firstEnemy);
			}

			animator?.SetBool("InCombatStance", true);
			
			OnReferenceStateChanged();
			onEnterCombat?.Invoke(this);
		}

		public void ExitCombat() {
			isInCombat = false;
			
			threats.Clear();

            if (animator != null) {
                animator?.SetBool("InCombatStance", false);
            }

			OnReferenceStateChanged();
			onExitCombat?.Invoke(this);
		}
		
		public bool IsLeftSwinging() {
			return animator.IsPlaying(1, "UnarmedSwing") || animator.IsPlaying(1, "ArmedOnehandSwing");
		}

		public bool IsRightSwinging() {
			return animator.IsPlaying(0, "UnarmedSwing") || animator.IsPlaying(0, "ArmedOnehandSwing");
		}

        public void BeginAttack() {
            IWeapon weapon = GetEquipment(EEquipSlot.Mainhand) as IWeapon;
            AnimationClip replaceClip = weapon?.AttackAnimation ?? defaultAttackAnimation;


            if (weapon?.CanFire() ?? true) {
                weapon?.StartFire(this);
                entityAnimator.SetAnimation(replaceableAnimationClip.name, replaceClip);
            }
        }

        public void UpdateAttack() {
            IWeapon weapon = GetEquipment(EEquipSlot.Mainhand) as IWeapon;
            weapon?.UpdateFire(this);
        }

        public void EndAttack() {
            IWeapon weapon = GetEquipment(EEquipSlot.Mainhand) as IWeapon;

            if (weapon?.CanFire() ?? true) {
                weapon?.EndFire(Id.Equals("ActorPlayer") ? Camera.main.transform : transform, this);
                animator.SetTrigger("SwingRight");
            }
        }

		public bool IsScared(Actor target) {
			bool result = false;

			switch (ActorModel.battleConfidence) {
				case EBattleConfidence.Afraid:
					result = true;
					break;
				case EBattleConfidence.Cautious:
					result = target.Level < Level;
					break;
				case EBattleConfidence.Average:
					result = target.Level <= Level;
					break;
				case EBattleConfidence.Heroic:
					result = target.Level <= (Level + HEROIC_ATTACK_BUFFER);
					break;
			}
			
			return result;
		}

		public bool InInterestDistance(WorldObject target) {
			return (Transform.position - target.Transform.position).sqrMagnitude <= Math.Pow(LoseInterestDistance, 2);
		}

		public void AddThreat(Actor actor) {
			threats.Add(actor);
		}

		public void RemoveThreat(Actor actor) {
			threats.Remove(actor);

			if (threats.Count == 0) {
				ExitCombat();
			}
		}
		
#endregion

#region Items and Inventory
		
		[ConsoleCommand("giveme", "Gives the player the item with the given id and quantity")]
		[ConsoleCommand("giveitem", "Gives the actor the item with the given id and quantity", ECommandTarget.LookAt)]
		public void GiveItem(string id, int amount) {
			ItemModel item = GameManager.AssetManager.GetById<ItemModel>(id);

			if (item == null) {
				return;
			}

			ActorModel.inventory.AddItem(id, amount, item.weight);
			onItemAdded?.Invoke(this, id, amount);
		}
		
		public bool TakeItem(string id, int amount) {
			int removed = ActorModel.inventory.RemoveItem(id, amount);

			if (removed != 0) {
				onItemRemoved?.Invoke(this, id, removed);
			}

            return removed != 0;
        }

        public int GetQuantity(string id) {
            ItemListEntry entry = Inventory[id];
            return entry?.count ?? 0;
        }

        public void ActivateItem(string id) {
			if (string.IsNullOrEmpty(id) || !Inventory.HasItem(id)) {
				return;
			}

			ItemModel itemModel = GameManager.AssetManager.GetById<ItemModel>(id);
			
			if (itemModel is ArmorModel || itemModel is WeaponModel) {
				Inventory.RemoveItem(id, 1);
				IEquippable item = GameManager.Spawn(itemModel, Vector3.zero, Quaternion.identity) as IEquippable;

				if (item != null) {
					Equip(item);
				}
			} else if (itemModel is ConsumableModel) {
				Inventory.RemoveItem(itemModel.id, 1);
				Consume((ConsumableModel)itemModel);
			}
		}

		public void Consume(ConsumableModel consumableModel) {
            if (consumableModel.statusEffects == null) {
                return;
            }
            
            foreach (StatusEffectModel model in consumableModel.statusEffects) {
                StatusEffect effect;

                switch (model.buffType) {
                    case Core.EBuffType.Healing:
                        effect = new HealingBuff(model, gameObject);
                        break;
                    case Core.EBuffType.Speed:
                        effect = new SpeedBuff(model, gameObject);
                        break;
                    default:
                        effect = null;
                        break;
                }

                if (effect != null) {
                    statusEffects.Add(effect);
                }
            }
		}

        /// <summary>
        /// Attempts to equip the item to the actor
        /// </summary>
        /// <param name="item">The equippable item to attach to the actor</param>
        /// <returns>Return true if the item is equipped, false otherwise</returns>
		public bool Equip(IEquippable item) {
            // If the item is empty, do nothing
			if (item == null) {
				return false;
			}

            // Make sure the object is not reacting to physics
            item.Rigidbody.isKinematic = true;
            item.Collider.enabled = false;

            if (item is IArmor) { // If this is armor
                IArmor armor = item as IArmor;

                // Set the parent and override the bones with the actors bones
                armor.Transform.SetParent(transform);
                armor.SetBones(meshRenderer);
            } else if (item is IWeapon) { // If this is a weapon
                IWeapon weapon = item as IWeapon;

                // Set the parent and reset the position and rotation to the actors hand
                weapon.Transform.SetParent(mainhandBone);
                weapon.Transform.localPosition = Vector3.zero;
                weapon.Transform.rotation = mainhandBone.rotation;

                // Provide an offhand IK target
                if (weapon.OffhandIkTarget != null) {
                    // TODO : Do some offhand IK stuff here
                }
            }

            // Unequip other pieces and store this item
            Unequip(item);
            equipment[(int)item.EquipSlot] = item;

            // Update the actor's animator
            animator.SetBool("HasWeapon", equipment[(int)EEquipSlot.Mainhand] != null);

			return true;
        }

        /// <summary>
        /// Provides unequip functionality for a specific type of equippable
        /// </summary>
        /// <param name="equippable">The equippable to attempt to unequip</param>
        protected bool Unequip(IEquippable equippable) {
            IWeapon weapon = equippable as IWeapon;
            bool unequipped = false;

            // If this is a two handed weapon
            if (weapon != null && weapon.WeaponType == EWeaponType.TwoHanded) {
                // Make sure the mainhand is unequipped
                if (equipment[(int)EEquipSlot.Mainhand] != null) {
                    unequipped = Unequip(EEquipSlot.Mainhand);
                }

                // Also make sure the offhand is unequipped
                if (equipment[(int)EEquipSlot.Offhand] != null) {
                    unequipped |= Unequip(EEquipSlot.Offhand);
                }
            } else {
                // If this is any other type of item, just make sure its slot is unequipped
                if (equipment[(int)equippable.EquipSlot] != null) {
                    unequipped = Unequip(equippable.EquipSlot);
                }
            }

            return unequipped;
        }

        /// <summary>
        /// Unequips an item from the given equipment spot and either drops it or adds it to the actor's inventory
        /// </summary>
        /// <param name="equipSlot">The equipment slot to unequip from</param>
        /// <param name="drop">Flag indicating if the item should be dropped</param>
        /// <returns>Return true if an item was unequipped, false otherwise</returns>
        public bool Unequip(EEquipSlot equipSlot, bool drop = false) {
            IEquippable item = equipment[(int)equipSlot];

            // If the item is null, do nothing
			if (item == null) {
				return false;
			}

            // Clean up the equipment slot
            equipment[(int)equipSlot] = null;

            if (item is IArmor) {
                IArmor armor = item as IArmor;

                // Reset the bones and make sure the armor is rendering
                armor.ClearBones();
                armor.Renderer.enabled = true;
            } else if (item is IWeapon) {
                IWeapon weapon = item as IWeapon;

                // Provide IK movement for the actor's offhand if needed
                if (weapon.OffhandIkTarget != null) {
                    // TODO : Do some offhand IK stuff here
                }
            }

            // If the item should be dropped
            if (drop) {
                // Reset the objects components
                item.Rigidbody.isKinematic = false;
                item.Collider.enabled = true;

                // Unparent the object
                item.Transform.SetParent(null);
            } else {
                // Add the item to the actor's inventory and get rid of the GameObject
                GiveItem(item.Id, 1);
                Destroy(item.Transform.gameObject);
            }

            // Update the actor's animator
			animator.SetBool("HasWeapon", equipment[(int)EEquipSlot.Mainhand] != null);

			return true;
        }

        public IEquippable GetEquipment(EEquipSlot slot) {
            return equipment[(int)slot];
        }
#endregion

        protected void ProcessStatusEffects(float deltaTime) {
            if (statusEffects == null) {
                return;
            }

            for (int i = statusEffects.Count - 1; i >= 0; i--) {
                if (!statusEffects[i].Tick(deltaTime)) {
                    statusEffects.RemoveAt(i);
                }
            }
        }

        private void OnStatusEffectDisolved(StatusEffect effect) {
            statusEffects.Remove(effect);
        }

		public override void Activate(object sender, ActivateEventArgs e) {
			if (e?.Activator != null) {
				return;
			}
			
			if (ReferenceEquals(GameManager.Player, e?.ActivatedBy)) {
				if (IsDead) {
					ContainerView view = UiManager.UI.OpenWindow<ContainerView>();
					view.View(e?.ActivatedBy as Actor, this);
				} else {
					DialogueView view = UiManager.UI.OpenWindow<DialogueView>();
					view.SetTarget(this);
				}
			}
		}
		
		public void SetRagdollActive(bool active) {
			foreach (Rigidbody body in ragdollRigidbodies) {
				body.useGravity = active;
				body.isKinematic = !active;
			}
		}
	}
}
