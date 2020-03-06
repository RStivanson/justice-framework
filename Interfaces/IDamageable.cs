using JusticeFramework.Components;

namespace JusticeFramework.Interfaces {
	public delegate void OnCurrentHealthChanged(IDamageable damagable);

	public interface IDamageable {
		event OnCurrentHealthChanged onCurrentHealthChanged;

		float CurrentHealth { get; }
		int MaxHealth { get; }

		void Damage(Actor attacker, float damageAmount, bool ignoreDamageReduction = false);
		void Heal(Actor healer, float amount);
	}
}