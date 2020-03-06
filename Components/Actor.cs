using JusticeFramework.Collections;
using JusticeFramework.Console;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using JusticeFramework.Managers;
using JusticeFramework.StatusEffects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Components {
    public delegate void OnEnterCombat(Actor actorNowInCombat);
	public delegate void OnExitCombat(Actor actorNoLongerInCombat);
	public delegate void OnLevelUp(Actor leveledUp, int newLevel);

	/// <summary>
	/// This class houses all model and functions relating for actors (NPCs and Player)
	/// </summary>
	[Serializable]
	public class Actor : WorldObject, IContainer, IDamageable {
		public const int HeroicAttackBuffer = 3;
		
		public event OnCurrentHealthChanged onCurrentHealthChanged;
		public event OnLevelUp onLevelUp;

		public event OnEnterCombat onEnterCombat;
		public event OnExitCombat onExitCombat;
        
        /// <summary>
        /// Entity animator that manages the first person rig
        /// </summary>
        [SerializeField]
        private PerspectiveAnimator actorAnimator;

        /// <summary>
        /// Reference storing all the actors equipment
        /// </summary>
        [SerializeField]
		private Equipment equipment;

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
		private List<Actor> threats;

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
        private GameEventData[] onKill;

        /// <summary>
        /// List of status effects currently applied to this actor
        /// </summary>
        [SerializeField]
        private List<StatusEffect> statusEffects;

        private Inventory inventory;

        public override EInteractionType InteractionType {
			get {
                if (IsInCombat)
                    return EInteractionType.None;
                return IsDead ? EInteractionType.Loot : EInteractionType.Talk;
            }
		}

		public Inventory Inventory {
			get { return inventory; }
		}
		
        public Equipment Equipment {
            get { return equipment; }
        }

		/// <summary>
		/// The base health of this actor
		/// </summary>
		public int BaseHealth {
			get { return GetData<ActorData>().BaseMaxHealth; }
		}
		
		/// <summary>
		/// The maximum health of this actor
		/// </summary>
		public int MaxHealth {
			get { return GetData<ActorData>().BaseMaxHealth + (Level * 25); }
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
			get { return (int)Math.Sqrt(((CurrentExperience - 125) * 100) / 125); }
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

                    if (GameManager.IsPlayer(this)) {
                        UiManager.Notify($"You are now level {newLevel}");
                    }
				}
			}
		}

		/// <summary>
		/// Flag stating if this actor is invincible (is killable)
		/// </summary>
		public bool IsInvincible {
			get { return GetData<ActorData>().IsInvincible; }
		}
		
		/// <summary>
		/// Flag stating if this actor is currently dead
		/// </summary>
		public bool IsDead {
			get {
                return currentHealth <= 0 && !GetData<ActorData>().IsInvincible;
            }
		}
		
		/// <summary>
		/// Flag stating if this actor is in combat
		/// </summary>
		public bool IsInCombat {
			get { return isInCombat; }
		}

		public List<Actor> Threats {
			get { return threats; }
		}

		/// <summary>
		/// The total armor rating of all equipped items
		/// </summary>
		public int TotalArmor {
			get {
				int totalArmorRating = 0;
				IArmor temp;

				for (EEquipSlot slot = EEquipSlot.Head; slot < EEquipSlot.Waist; slot++) {
					temp = equipment.Get<IArmor>(slot);

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
                IWeapon weapon = equipment.Get<IWeapon>(EEquipSlot.Mainhand);
                return weapon != null ? weapon.Damage : 5;
			}
		}

        public bool IsAttacking { get; set; }
        
		/// <inheritdoc cref="WorldObject" />
		protected override void OnIntialized() {
			ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            statusEffects = new List<StatusEffect>();

			threats = new List<Actor>();
            equipment = new Equipment();
            inventory = new Inventory();

            ResetToDefaultItems();
            ExitCombat();
		}

        /*protected virtual void Update() {
            ProcessStatusEffects(Time.deltaTime);
        }*/

		private void OnDrawGizmosSelected() {
            if (dataObject == null) {
                return;
            }

            ActorData data = dataObject as ActorData;

            // Draw detection radius
            Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(data.AiData.LoseInterestDistanceSqr));
		}

        #region Health Functions

        /*[ConsoleCommand("kill", "Kills the target actor unless it is invincible", ECommandTarget.LookAt)]
		public void Kill() {
			Damage(null, currentHealth, true);
		}*/
		
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

            ActorData data = dataObject as ActorData;
			if (currentHealth <= 0) {
				if (data.IsInvincible) {
					currentHealth = 1;
				} else {
					currentHealth = 0;
                    GameEvent.ExecuteAll(onKill, this, attacker);
					ExitCombat();
				}
			}

			if (currentHealth > MaxHealth) {
				currentHealth = MaxHealth;
			}

			OnReferenceStateChanged();
			onCurrentHealthChanged?.Invoke(this);
		}

		public void Heal(Actor healer, float amount) {
			currentHealth += amount;

            ActorData data = dataObject as ActorData;
            if (currentHealth <= 0) {
				if (data.IsInvincible) {
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
			onCurrentHealthChanged?.Invoke(this);
		}
		
        #endregion
		
        #region Experience Functions

		/*[ConsoleCommand("addexp", "Adds experience to the target actor", ECommandTarget.LookAt)]
		public void AddExperience(int amount) {
            if (GameManager.IsPlayer(this)) {
                GameManager.Notify($"You gained {amount} experience");
            }

            CurrentExperience = CurrentExperience + amount;

            OnReferenceStateChanged();
		}
		
		[ConsoleCommand("removeexp", "Removes experience from the target actor", ECommandTarget.LookAt)]
		public void RemoveExperience(int amount) {
            if (GameManager.IsPlayer(this)) {
                GameManager.Notify($"You lost {amount} experience");
            }

            CurrentExperience = CurrentExperience - amount;

            OnReferenceStateChanged();
		}*/
		
        #endregion

        #region Combat
		
		public void EnterCombat(Actor firstEnemy = null) {
			isInCombat = true;
			threats.Clear();
			
			if (firstEnemy != null) {
				threats.Add(firstEnemy);
			}

            actorAnimator.SetBool("IsInCombat", true);

            OnReferenceStateChanged();
			onEnterCombat?.Invoke(this);
		}

		public void ExitCombat() {
			isInCombat = false;
			
			threats.Clear();

            actorAnimator.SetBool("IsInCombat", false);

            OnReferenceStateChanged();
			onExitCombat?.Invoke(this);
		}
		
        public void BeginAttack() {
            IWeapon weapon = equipment.Get<IWeapon>(EEquipSlot.Mainhand);

            if (weapon?.CanFire() ?? true) {
                weapon?.StartFire(this);

                IsAttacking = true;
            }
        }

        public EAttackStatus UpdateAttack() {
            IWeapon weapon = equipment.Get<IWeapon>(EEquipSlot.Mainhand);
            return weapon?.UpdateFire(this) ?? EAttackStatus.Empty;
        }

        public void EndAttack() {
            IWeapon weapon = equipment.Get<IWeapon>(EEquipSlot.Mainhand);

            if ((weapon?.CanFire() ?? true) || IsAttacking) {
                weapon?.EndFire(GameManager.IsPlayer(this) ? Camera.main.transform : transform, this);
                actorAnimator.SetTrigger("Attack");
            }
        }
        
		public bool IsScaredOf(Actor target) {
			bool result = false;

            ActorData data = dataObject as ActorData;
            switch (data.AiData.Confidence) {
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
					result = target.Level <= (Level + HeroicAttackBuffer);
					break;
			}
			
			return result;
		}

		public bool InInterestDistance(WorldObject target) {
            ActorData data = dataObject as ActorData;
            return (Transform.position - target.Transform.position).sqrMagnitude <= data.AiData.LoseInterestDistanceSqr;
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

        public void ResetToDefaultItems() {
            ChestData data = dataObject as ChestData;

            if (data == null) {
                return;
            }

            Inventory.Clear();

            foreach (ItemStackData isd in data.DefaultInventory) {
                Inventory.Add(isd.itemData.Id, isd.quantity);
            }
        }

        /// <summary>
        /// Adds the given item to the actor's inventory
        /// </summary>
        /// <param name="id">The Id of the item to add</param>
        /// <param name="amount">The quantity of item to add</param>
		[ConsoleCommand("giveme", "Gives the player the item with the given id and quantity")]
		[ConsoleCommand("giveitem", "Gives the actor the item with the given id and quantity", ECommandTarget.LookAt)]
		private void GiveItem(string id, int amount) {
            // If the item doesn't exist, don't add it
			if (!GameManager.DataManager.IsAssetLoaded<ItemData>(id)) {
                return;
            }

            // Add the item to the inventory
            Inventory.Add(id, amount);
        }
		
        public void ActivateItem(string id) {
			if (!Inventory.Contains(id)) {
				return;
			}

			ItemData ItemData = GameManager.DataManager.GetAssetById<ItemData>(id);
			
			if (ItemData is ArmorData || ItemData is WeaponData) {
				Inventory.Remove(id, 1);
                // TODO
                IEquippable item = null; // GameManager.SpawnEquipment(ItemData as EquippableModel);

				if (item != null) {
                    equipment.Equip(item, mainhandBone, actorAnimator, meshRenderer);
				}
			} else if (ItemData is PotionData potion) {
				Inventory.Remove(potion.Id, 1);
				Consume(potion);
			}
		}

		public void Consume(PotionData consumableModel) {
            if (consumableModel.StatusEffects == null) {
                return;
            }
            
            foreach (StatusEffectData model in consumableModel.StatusEffects) {
                StatusEffect effect;

                switch (model.BuffType) {
                    case EBuffType.Healing:
                        effect = new HealingBuff(model, gameObject);
                        break;
                    case EBuffType.Speed:
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

        public static bool Equip(Actor target, Inventory inventory, string id, int quantity = 1) {
            if (!inventory.Contains(id)) {
                return false;
            }

            inventory.Remove(id, quantity);

            IEquippable equippable = GameManager.SpawnAtPlayer(id) as IEquippable;
            equippable.StackAmount = quantity;

            return Equip(target, equippable);
        }

        public static bool Equip(Actor target, IEquippable equippable) {
            if (target.mainhandBone == null) {
                Debug.Log(target.DisplayName);
                return false;
            }

            equippable = target.Equipment.Equip(equippable, target.mainhandBone, target.actorAnimator, target.meshRenderer);

            if (equippable != null) {
                target.Inventory.Add(equippable.Id, equippable.StackAmount);
            }

            return true;
        }

        public static void Unequip(Actor target, Inventory inventory, EEquipSlot slot) {
            IEquippable equippable = target.Equipment.Unequip(slot);

            if (equippable != null && inventory != null) {
                inventory.Add(equippable.Id, equippable.StackAmount);
                Destroy(equippable.Transform.gameObject);
            }
        }

        #endregion

        /*protected void ProcessStatusEffects(float deltaTime) {
            if (statusEffects == null) {
                return;
            }

            for (int i = statusEffects.Count - 1; i >= 0; i--) {
                if (!statusEffects[i].Tick(deltaTime)) {
                    statusEffects.RemoveAt(i);
                }
            }
        }*/

        protected override Logic.Action OnActivate(IWorldObject activator) {
            Logic.Action action = null;

            if (IsInCombat) {
                action = new ActionEmpty();
                return action;
            }

            if (IsDead) {
                action = new ActionOpen(this);
                return action;
            }

            action = new ActionTalk(this);
            return action;
		}
		
		public void SetRagdollActive(bool active) {
			foreach (Rigidbody body in ragdollRigidbodies) {
				body.useGravity = active;
				body.isKinematic = !active;
			}

            OnReferenceStateChanged();
		}
	}
}
