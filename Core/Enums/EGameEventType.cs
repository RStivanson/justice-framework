namespace JusticeFramework.Core {
    /// <summary>
    /// Enumeration defining the type of game events
    /// </summary>
    public enum EGameEventType {
        /// <summary>
        /// Event where the target is given an item
        /// </summary>
        GiveItem,

        /// <summary>
        /// Event where the target has an item taken
        /// </summary>
        TakeItem,

        /// <summary>
        /// Event where the target quest is advanced
        /// </summary>
        AdvanceQuest,

        /// <summary>
        /// Opens a merchant window
        /// </summary>
        OpenMerchantWindow,
    }
}
