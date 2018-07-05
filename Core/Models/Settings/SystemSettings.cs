using JusticeFramework.Core.Managers;
using System;
using System.IO;
using UnityEngine;

namespace JusticeFramework.Core.Models.Settings {
    /// <summary>
    /// Holds all current settings and functionality to load, save and update them
    /// </summary>
    [Serializable]
    public static class SystemSettings {
        /// <summary>
        /// Data class that holds all current settings
        /// </summary>
        [SerializeField]
        private static SystemSettingsModel settings;

        /// <summary>
        /// Gets or sets the system settings
        /// </summary>
        public static SystemSettingsModel Settings {
            get { return settings; }
            set {
                settings = value;

                // Make sure we apply the new settings after receiving them
                ApplySettings();
            }
        }

        /// <summary>
        /// Applys current settings to the engine
        /// </summary>
        public static void ApplySettings() {
            // Set the resolution
            Screen.SetResolution(settings.resolutionWidth, settings.resolutionHeight, settings.useFullscreen, settings.resolutionRefreshRate);

            // Set the vSync flag
            QualitySettings.vSyncCount = settings.useVSync ? 1 : 0;

            // Set the master volume
            AudioListener.volume = settings.masterVolume;
            GameManager.AmbientSource.volume = GetScaledVolume(1, EAudioType.Ambient);
        }

        /// <summary>
        /// Scales the volume value
        /// </summary>
        /// <param name="value">The value to be scaled</param>
        /// <returns>Returns a volume value scaled based on its type</returns>
        public static float GetScaledVolume(float value, EAudioType audioType) {
            float scale;

            // Get the scale of the audio based on the audio type
            switch (audioType) {
                case EAudioType.Ambient:
                    scale = settings.ambientVolume;
                    break;
                case EAudioType.Dialogue:
                    scale = settings.dialogueVolume;
                    break;
                case EAudioType.SoundEffect:
                    scale = settings.soundEffectVolume;
                    break;
                default:
                    scale = 1;
                    break;
            }
            
            // Return the final scaled value
            return value * scale;
        }

        #region IO

        /// <summary>
        /// Sets default settings and saves them
        /// </summary>
        /// <param name="path">The path location to the saved data</param>
        /// <param name="filename">The file name of the saved data</param>
        public static void CreateNew(string path, string filename) {
            // Create a new settings object
            settings = new SystemSettingsModel();
            settings.SetDefaults();

            // Save the new data
            Save(path, filename);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The path location to the saved data</param>
        /// <param name="filename">The file name of the saved data</param>
        /// <param name="createIfDoesntExist">Flag indicating if new settings should be created if none exist</param>
        /// <returns>Returns true if settings were loaded, false otherwise</returns>
        public static bool Load(string path, string filename, bool createIfDoesntExist = false) {
            // Get the full path
            string fullPath = Path.Combine(path, filename);

            // Check if the file exists
            if (!File.Exists(fullPath)) {
                // If we want to create new settings then create them
                if (createIfDoesntExist) {
                    CreateNew(path, filename);
                }

                return false;
            }

            // Read the settings file and parse it
            string settingsJson = File.ReadAllText(fullPath);
            settings = JsonUtility.FromJson<SystemSettingsModel>(settingsJson);

            return true;
        }

        /// <summary>
        /// Saves the current settings to the file
        /// </summary>
        /// <param name="path">The path location to the saved data</param>
        /// <param name="filename">The file name of the saved data</param>
        public static void Save(string path, string filename) {
            // Get the path and settings json
            string fullPath = Path.Combine(path, filename);
            string json = JsonUtility.ToJson(settings);

            // Write to the file
            File.WriteAllText(fullPath, json);
        }

        #endregion
    }
}
