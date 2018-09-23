namespace JusticeFramework.Core.Interfaces {
	/// <inheritdoc cref="IContainer" />
	/// <summary>
	/// Interface that defines attributes needed for Actors
	/// </summary>
	public interface IActor : IWorldObject, IContainer {
		/// <summary>
		/// The base amount of health that this actor has before any bonuses are applied
		/// </summary>
		int BaseHealth { get; }
		
		/// <summary>
		/// The maximum amount of health that this actor has
		/// </summary>
		int MaxHealth { get; }
		
		/// <summary>
		/// The current amount of health that this actor has
		/// </summary>
		float CurrentHealth { get; set; }
		
		/// <summary>
		/// The current amount of experience that this actor has
		/// </summary>
		int CurrentExperience { get; set; }
		
		/// <summary>
		/// Flag indicating if this actor is invincible
		/// </summary>
		bool IsInvincible { get; set; }
		
		/// <summary>
		/// Flag indicating if this actor id dead
		/// </summary>
		bool IsDead { get; }

		/// <summary>
		/// The amount of confidence that this actor has in battle
		/// </summary>
		EBattleConfidence BattleConfidence { get; set; }
		
		/// <summary>
		/// The level of aggressiveness shown by this actor near other NPCs
		/// </summary>
		EAggression Aggression { get; set; }
		
		/// <summary>
		/// The level of morality tolerated by this actor
		/// </summary>
		EMorals Morals { get; set; }
		
		/// <summary>
		/// Where the actor checks distance against when determining if it should still chase its target
		/// </summary>
		EInterestOrigin InterestOrigin { get; set; }
		
		/// <summary>
		/// The amount of distance that needs to be between the interest origin and the target before interest is lost
		/// </summary>
		float LoseInterestDistance { get; set; }

        bool IsInCombat { get; }

        void EnterCombat(IActor firstEnemy = null);

        void ExitCombat();

        /// <summary>
        /// Attempts to to attack the target by punching, swinging, or shooting
        /// </summary>
        void BeginAttack();

        EAttackStatus UpdateAttack();

        void EndAttack();

		/// <summary>
		/// Adds experience to the actor
		/// </summary>
		/// <param name="amountOfExperienceToAdd">The amount of experience to add</param>
		void AddExperience(int amountOfExperienceToAdd);
	}
}