namespace JusticeFramework {
	/// <summary>
	/// Difficulty of lockpicking a lock
	/// </summary>
	public enum ELockDifficulty {
        /// <summary>
        /// Easiest pickable difficulty.
        /// </summary>
		Beginner = 0,

        /// <summary>
        /// Easy pickable difficulty.
        /// </summary>
		Novice = 1,

        /// <summary>
        /// Average pickable difficulty.
        /// </summary>
		Apprentice = 2,

        /// <summary>
        /// Challenging pickable diffulty.
        /// </summary>
		Expert = 3,

        /// <summary>
        /// Hardest pickable difficulty.
        /// </summary>
		Master = 4,

        /// <summary>
        /// An impossible lock cannot be picklocked.
        /// </summary>
        Impossible = 5
	}
}