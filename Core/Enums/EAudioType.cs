namespace JusticeFramework.Core {
    /// <summary>
    /// The type of audio used throughout the game
    /// </summary>
    public enum EAudioType {
        /// <summary>
        /// General background noise and music
        /// </summary>
        Ambient,
        
        /// <summary>
        /// Noise made by an actor when talking to them
        /// </summary>
        Dialogue,

        /// <summary>
        /// Noise made by objects such as levers, casting spells, or drinking potions
        /// </summary>
        SoundEffect
    }
}