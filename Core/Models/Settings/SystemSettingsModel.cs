using System;
using UnityEngine;

namespace JusticeFramework.Core.Models.Settings {
    /// <summary>
    /// Data class to hold all data relating to game settings. Used for json serialization
    /// </summary>
    [Serializable]
    public class SystemSettingsModel {
        /// <summary>
        /// The screen resolution width
        /// </summary>
        public int resolutionWidth;

        /// <summary>
        /// The screen resolution height
        /// </summary>
        public int resolutionHeight;

        /// <summary>
        /// The targeted resolution refresh rate
        /// </summary>
        public int resolutionRefreshRate;

        /// <summary>
        /// Flag indicating if the game is being played in fullscreen
        /// </summary>
        public bool useFullscreen;

        /// <summary>
        /// Flag indicating if vSync should be used
        /// </summary>
        public bool useVSync;
        
        /// <summary>
        /// Level of the master volume
        /// </summary>
        public float masterVolume;

        /// <summary>
        /// Volume level of ambient noise/music 
        /// </summary>
        public float ambientVolume;

        /// <summary>
        /// Volume level of dialogue speech
        /// </summary>
        public float dialogueVolume;

        /// <summary>
        /// Volume level of sound effects
        /// </summary>
        public float soundEffectVolume;

        /// <summary>
        /// Sets the default settings values
        /// </summary>
        public void SetDefaults() {
            resolutionWidth = Screen.currentResolution.width;
            resolutionHeight = Screen.currentResolution.height;
            resolutionRefreshRate = Screen.currentResolution.refreshRate;

            useFullscreen = true;
            useVSync = false;

            masterVolume = 1;
            ambientVolume = 1;
            dialogueVolume = 1;
            soundEffectVolume = 1;
        }
    }
}
