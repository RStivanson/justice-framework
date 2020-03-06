namespace JusticeFramework {
    /// <summary>
    /// The type of audio used throughout the game
    /// </summary>
    public enum EAudioType {
        /// <summary>
        /// Noise made by objects such as levers, casting spells, or drinking potions
        /// </summary>
        SoundEffect = 0,

        /// <summary>
        /// General background noise and music
        /// </summary>
        Ambient = 1,
        
        /// <summary>
        /// Noise made by an actor when talking to them
        /// </summary>
        Dialogue = 2,
    }
}