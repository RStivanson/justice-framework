namespace JusticeFramework {
    /// <summary>
    /// The method used for selecting a response in a dialogue topic.
    /// </summary>
    public enum EResponseSelectMethod {
        /// <summary>
        /// Selects the first response that meets the conditions
        /// </summary>
        FirstMatch = 0,

        /// <summary>
        /// Selects a random response from all the responses that meets the conditions
        /// </summary>
        Random,
    }
}
