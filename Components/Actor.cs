using System;
using System.Collections.Generic;
using JusticeFramework.AI;
using JusticeFramework.Console;
using JusticeFramework.Data.Models;
using JusticeFramework.Data.Collections;
using JusticeFramework.Data;
using JusticeFramework.Data.Events;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Interfaces;
using JusticeFramework.UI.Views;
using JusticeFramework.Utility.Extensions;
using UnityEngine;

namespace JusticeFramework.Components {
	public delegate void OnEnterCombat(Actor actorNowInCombat);
	public delegate void OnExitCombat(Actor actorNoLongerInCombat);
	public delegate void OnLevelUp(Actor leveledUp, int newLevel);

	/// <summary>
	/// This class houses all model and functions relating for actors (NPCs and Player)
	/// </summary>
	[Serializable]
	[RequireComponent(typeof(AiVision))]
	[RequireComponent(typeof(AiController))]
	[RequireComponent(typeof(AudioSource))]
	public class Actor : Reference, IActor, IDamageable, IInventory {
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
		/// The AI vision component
		/// </summary>
		private AiVision aiVision;
		
		/// <summary>
		/// The AI controller component
		/// </summary>
		private AiController aiController;
		
		/// <summary>
		/// The attached animator component
		/// </summary>
		[SerializeField]
		private Animator animator;
		
		/// <summary>
		/// Equipment array storing all equipped items
		/// </summary>
		[SerializeField]
		private IEquippable[] equipment;
		
		/// <summary>
		/// SkinnedMeshRenderer attached to the Body mesh
		/// </summary>
		public SkinnedMeshRenderer bodySMR;

		/// <summary>
		/// SkinnedMeshRenderer attached to the Head mesh
		/// </summary>
		public SkinnedMeshRenderer headSMR;
		
		/// <summary>
		/// The current health value of this actor
		/// </summary>
		public float currentHealth = 100;

		/// <summary>
		/// The amount of experience held by the actor
		/// </summary>
		public int currentExperience = 0;
		
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
		/// The audio source component attached to this object
		/// </summary>
		[SerializeField]
		private AudioSource audioSource;

		/// <summary>
		/// The rigidbody components attached to all pieces of the GameObject
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private Rigidbody[] ragdollRigidbodies;

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

		public ItemList Inventory {
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

		public List<Actor> Threats {
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

		/// <inheritdoc cref="Reference" />
		protected override void OnIntialize() {
			animator = GetComponent<Animator>();
			audioSource = GetComponent<AudioSource>();
			
			ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

			threats = new List<Actor>();
			equipment = new IEquippable[Enum.GetNames(typeof(EEquipSlot)).Length];
			
			Equip(GameManager.Spawn("TestHelm001") as IEquippable);
			Equip(GameManager.Spawn("TestChestplate001") as IEquippable);
			Equip(GameManager.Spawn("TestPlatelegs001") as IEquippable);
			Equip(GameManager.Spawn("TestSword001") as IEquippable);

			ExitCombat();
		}

		private void OnDrawGizmosSelected() {
			// Draw detection radius
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, LoseInterestDistance);
		}

#region Health Functions
		
		[ConsoleCommand("kill", "Kills the target actor unless it is invincible", ECommandTarget.LookAt)]
		public void Kill() {
			Damage(null, currentHealth, true);
		}
		
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
		
		public void EnterCombat(Actor firstEnemy = null) {
			isInCombat = true;
			threats.Clear();
			
			if (firstEnemy != null) {
				threats.Add(firstEnemy);
			}

			animator.SetBool("InCombatStance", true);
			
			OnReferenceStateChanged();
			onEnterCombat?.Invoke(this);
		}

		public void ExitCombat() {
			isInCombat = false;
			
			threats.Clear();

			animator.SetBool("InCombatStance", false);

			OnReferenceStateChanged();
			onExitCombat?.Invoke(this);
		}
		
		public bool IsLeftSwinging() {
			return animator.IsPlaying(1, "UnarmedSwing") || animator.IsPlaying(1, "ArmedOnehandSwing");
		}

		public bool IsRightSwinging() {
			return animator.IsPlaying(0, "UnarmedSwing") || animator.IsPlaying(0, "ArmedOnehandSwing");
		}

		public void Swing() {
			animator.SetTrigger("SwingRight");
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

		public bool InInterestDistance(Reference target) {
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
			ItemModel item = GameManager.AssetManager.GetEntityById<ItemModel>(id);

			if (item == null) {
				return;
			}

			ActorModel.inventory.AddItem(id, amount, item.weight);
			onItemAdded?.Invoke(this, id, amount);
		}
		
		public void TakeItem(string id, int amount) {
			int removed = ActorModel.inventory.RemoveItem(id, amount);

			if (removed != 0) {
				onItemRemoved?.Invoke(this, id, removed);
			}
		}
		
		public void ActivateItem(string id) {
			if (string.IsNullOrEmpty(id) || !Inventory.HasItem(id)) {
				return;
			}

			ItemModel itemModel = GameManager.AssetManager.GetEntityById<ItemModel>(id);
			
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
			Heal(null, consumableModel.healthModifier);
		}

		public bool Equip(IEquippable item) {
			if (item == null) {
				return false;
			}

			item.Equip(this);
				
			equipment[(int)item.EquipSlot] = item;

			animator.SetBool("HasWeapon", equipment[(int)EEquipSlot.Mainhand] != null);

			return true;

		}

		public bool Unequip(EEquipSlot equipSlot) {
			int index = (int)equipSlot;

			if (equipment[index] == null) {
				return false;
			}

			equipment[index].Unequip(this);
			equipment[index] = null;
			animator.SetBool("HasWeapon", equipment[(int)EEquipSlot.Mainhand] != null);

			return true;

		}
		
#endregion

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
		
		public void PlaySound(AudioClip sound) {
			audioSource.PlayOneShot(sound);
		}
		
		public void SetRagdollActive(bool active) {
			foreach (Rigidbody body in ragdollRigidbodies) {
				body.useGravity = active;
				body.isKinematic = !active;
			}
		}
	}
}
