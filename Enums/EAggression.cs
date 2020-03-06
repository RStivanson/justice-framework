namespace JusticeFramework {
	/// <summary>
	/// The aggression ranking of an Actor. Determines how the Actor will behave when
	/// presented with a possible enemy.
	/// </summary>
	public enum EAggression {
		/// <summary>
		/// Will not attack anyone unless provoked
		/// </summary>
		Passive = 0,
		
		/// <summary>
		/// Will attack known enemies
		/// </summary>
		Aggressive,
		
		/// <summary>
		/// Will attack enemies or neutral foes
		/// </summary>
		VeryAggressive,
		
		/// <summary>
		/// Will attack anyone
		/// </summary>
		Berserk,
	}
}