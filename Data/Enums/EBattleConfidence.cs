namespace JusticeFramework.Data {
	/// <summary>
	/// The confidence ranking of an Actor. Determines how the Actor will react to combat
	/// scenarios with enemies.
	/// </summary>
	public enum EBattleConfidence {
		/// <summary>
		/// Will run away from combat regardless of enemy level
		/// </summary>
		Afraid = 0,
		
		/// <summary>
		/// Will proceed into combat with enemies with a level lower than its own
		/// </summary>
		Cautious = 10,
		
		/// <summary>
		/// Will proceed into combat with enemies of the same level or below
		/// </summary>
		Average = 15,
		
		/// <summary>
		/// Will proceed into combat with enemies of the same level plus a buffer or below
		/// </summary>
		Heroic = 20,
		
		/// <summary>
		/// Will proceed into combat with enemies of any level
		/// </summary>
		Fearless = 25,
	}
}