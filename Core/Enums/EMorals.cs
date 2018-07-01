using System;

namespace JusticeFramework.Core {
	/// <summary>
	/// Type of actions tolerated
	/// </summary>
	[Flags]
	public enum EMorals {
		/// <summary>
		/// Tolerates no crimes
		/// </summary>
		NoCrime = 1,
		
		/// <summary>
		/// Tolerates all crimes
		/// </summary>
		NoMorals = 2,
		
		/// <summary>
		/// Tolerates violence against enemies
		/// </summary>
		ViolenceAgainstEnemies = 4,
		
		/// <summary>
		/// Tolerates violence against all
		/// </summary>
		ViolenceAgainstAll = 8,
		
		/// <summary>
		/// Tolerates theft against enemies
		/// </summary>
		TheftAgainstEnemies = 16,
		
		/// <summary>
		/// Tolerates theft against all
		/// </summary>
		TheftAgainstAll = 32,
	}
}