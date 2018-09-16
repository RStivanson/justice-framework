using JusticeFramework.Core;
using JusticeFramework.Core.Collections;
using JusticeFramework.Core.Console;
using JusticeFramework.Core.Events;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models;
using JusticeFramework.Core.Models.Settings;
using JusticeFramework.Core.StatusEffects;
using JusticeFramework.Interfaces;
using JusticeFramework.StatusEffects;
using JusticeFramework.UI.Views;
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
	public class Actor : WorldObject, IActor, IDamageable {
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
		private List<IActor> threats;

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
		
        public Equipment Equipment {
            get { return equipment; }
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

        public bool IsPlayer {
            get { return Id.Equals(SystemConstants.AssetDataPlayerId); }
        }

#endregion

		/// <inheritdoc cref="WorldObject" />
		protected override void OnIntialized() {
			ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            statusEffects = new List<StatusEffect>();

			threats = new List<IActor>();

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

            actorAnimator.SetBool(SystemConstants.AnimatorIsInCombatParam, true);

            OnReferenceStateChanged();
			onEnterCombat?.Invoke(this);
		}

		public void ExitCombat() {
			isInCombat = false;
			
			threats.Clear();

            actorAnimator.SetBool(SystemConstants.AnimatorIsInCombatParam, false);

            OnReferenceStateChanged();
			onExitCombat?.Invoke(this);
		}
		
        public void BeginAttack() {
            IWeapon weapon = equipment.Get<IWeapon>(EEquipSlot.Mainhand);

            if (weapon?.CanFire() ?? true) {
                weapon?.StartFire(this);
            }
        }

        public void UpdateAttack() {
            IWeapon weapon = equipment.Get<IWeapon>(EEquipSlot.Mainhand);
            weapon?.UpdateFire(this);
        }

        public void EndAttack() {
            IWeapon weapon = equipment.Get<IWeapon>(EEquipSlot.Mainhand);

            if (weapon?.CanFire() ?? true) {
                weapon?.EndFire(IsPlayer ? Camera.main.transform : transform, this);
                actorAnimator.SetTrigger(SystemConstants.AnimatorAttackParam);
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
		
        /// <summary>
        /// Adds the given item to the actor's inventory
        /// </summary>
        /// <param name="id">The Id of the item to add</param>
        /// <param name="amount">The quantity of item to add</param>
		[ConsoleCommand("giveme", "Gives the player the item with the given id and quantity")]
		[ConsoleCommand("giveitem", "Gives the actor the item with the given id and quantity", ECommandTarget.LookAt)]
		private void GiveItem(string id, int amount) {
            // If the item doesn't exist, don't add it
			if (!GameManager.AssetManager.Contains<ItemModel>(id)) {
                return;
            }

            // Add the item to the inventory
            Inventory.Add(id, amount);
		}
		
        public void ActivateItem(string id) {
			if (!Inventory.Contains(id)) {
				return;
			}

			ItemModel itemModel = GameManager.AssetManager.GetById<ItemModel>(id);
			
			if (itemModel is EquippableModel) {
				Inventory.Remove(id, 1);
				IEquippable item = GameManager.Spawn(itemModel, Vector3.zero, Quaternion.identity) as IEquippable;

				if (item != null) {
                    equipment.Equip(item, mainhandBone, actorAnimator, meshRenderer);
				}
			} else if (itemModel is ConsumableModel) {
				Inventory.Remove(itemModel.id, 1);
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
