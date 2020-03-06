using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Setting object that contains data for overall game settings such as resolution and volume
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Settings Data", order = 152)]
    public class SettingsData : ScriptableObject {
        /// <summary>
        /// Screen resolution width
        /// </summary>
        public int screenWidth;

        /// <summary>
        /// Screen resolution height
        /// </summary>
        public int screenHeight;

        /// <summary>
        /// Screen resolution refresh rate
        /// </summary>
        public int screenRefreshRate;

        /// <summary>
        /// Flag that indicates if the game should run in fullscreen mode
        /// </summary>
        public bool useFullscreen;

        /// <summary>
        /// Flag that indicates if the game should use vSync
        /// </summary>
        public bool useVSync;

        /// <summary>
        /// Overall volume level that scales all other volume levels
        /// </summary>
        [Range(0, 1)]
        public float masterVolume;

        /// <summary>
        /// Volume level for game music and background sounds
        /// </summary>
        [Range(0, 1)]
        public float ambientVolume;

        /// <summary>
        /// Volume level for voice sounds
        /// </summary>
        [Range(0, 1)]
        public float voiceVolume;

        /// <summary>
        /// Sound level for sound effects
        /// </summary>
        [Range(0, 1)]
        public float sfxVolume;
    }
}
