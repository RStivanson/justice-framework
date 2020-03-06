using System;

namespace JusticeFramework {
    /// <summary>
    /// States that a quest can be in
    /// </summary>
    public enum EQuestState {
        /// <summary>
        /// The quest has not started yet
        /// </summary>
        NotStarted,

        /// <summary>
        /// The quest has been started and has not been completed or failed
        /// </summary>
        InProgress,

        /// <summary>
        /// The quest has been successfully complete
        /// </summary>
        Completed,

        /// <summary>
        /// The quest has been failed
        /// </summary>
        Failed
    }
}