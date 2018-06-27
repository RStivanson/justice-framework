namespace JusticeFramework.Core {
	/// <summary>
	/// Type of door behaviour
	/// </summary>
	public enum EDoorType {
		/// <summary>
		/// The door stays in place, and opens
		/// </summary>
		Static,
		
		/// <summary>
		/// The door transports the player to a new scene
		/// </summary>
		Portal,
	}
}