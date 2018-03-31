using JusticeFramework.Data.Collections;

namespace JusticeFramework.Data.Interfaces {
	public interface IActor : IWorldObject, IContainer {
		int BaseHealth { get; }
		int MaxHealth { get; }
		float CurrentHealth { get; set; }
		int CurrentExperience { get; set; }
		bool IsInvincible { get; set; }
		bool IsDead { get; }
		
		EBattleConfidence BattleConfidence { get; set; }
		EAggression Aggression { get; set; }
		EMorals Morals { get; set; }
		EInterestOrigin InterestOrigin { get; set; }
		float LoseInterestDistance { get; set; }

		void AddExperience(int amountOfExperienceToAdd);
	}
}