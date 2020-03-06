namespace JusticeFramework {
    /// <summary>
    /// Enumeration defining the type of game events
    /// </summary>
    public enum EGameEventType {
        /// <summary>
        /// Gives the target the specified item and quantity.
        /// </summary>
        GiveItem,

        /// <summary>
        /// Takes the specified item and quantity from the target.
        /// </summary>
        TakeItem,

        /// <summary>
        /// Sets the target quest to the specified stage marker.
        /// </summary>
        SetQuestStage,
    }
}
