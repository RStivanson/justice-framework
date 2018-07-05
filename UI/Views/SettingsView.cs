using JusticeFramework.Core;
using JusticeFramework.Core.Models.Settings;
using JusticeFramework.Core.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    [Serializable]
    public class SettingsView : Window {
        [SerializeField]
        [HideInInspector]
        private SystemSettingsModel settings;

        [SerializeField]
        private GameObject displayContainer;

        [SerializeField]
        private GameObject audioContainer;

        [SerializeField]
        private Dropdown resolutionDropdown;

        [SerializeField]
        private Toggle useVSyncToggle;

        [SerializeField]
        private Slider masterVolumeSlider;

        [SerializeField]
        private Slider ambientVolumeSlider;

        [SerializeField]
        private Slider dialogueVolumeSlider;

        [SerializeField]
        private Slider sfxVolumeSlider;

        public void Apply() {
            settings.resolutionWidth = Screen.resolutions[resolutionDropdown.value].width;
            settings.resolutionHeight = Screen.resolutions[resolutionDropdown.value].height;
            settings.resolutionRefreshRate = Screen.resolutions[resolutionDropdown.value].refreshRate;
            settings.useVSync = useVSyncToggle.isOn;

            settings.masterVolume = masterVolumeSlider.value;
            settings.ambientVolume = ambientVolumeSlider.value;
            settings.dialogueVolume = dialogueVolumeSlider.value;
            settings.soundEffectVolume = sfxVolumeSlider.value;

            SystemSettings.ApplySettings();
        }

        private void UpdateOptionValues(SystemSettingsModel settings) {
            // Display
            int selectedResolution = GetResolutionIndex(settings.resolutionWidth, settings.resolutionHeight, settings.resolutionRefreshRate);
            if (selectedResolution == -1) {
                selectedResolution = GetResolutionIndex(Screen.currentResolution.width, Screen.currentResolution.height, Screen.currentResolution.refreshRate);
            }

            resolutionDropdown.value = selectedResolution;
            useVSyncToggle.isOn = settings.useVSync;

            // Audio
            masterVolumeSlider.value = settings.masterVolume;
            ambientVolumeSlider.value = settings.ambientVolume;
            dialogueVolumeSlider.value = settings.dialogueVolume;
            sfxVolumeSlider.value = settings.soundEffectVolume;
        }

        public void ShowDisplayOptions() {
            HideAllCategories();
            displayContainer.SetActive(true);
        }

        public void ShowAudioOptions() {
            HideAllCategories();
            audioContainer.SetActive(true);
        }

        private void HideAllCategories() {
            displayContainer.SetActive(false);
            audioContainer.SetActive(false);
        }

        #region Event Callbacks

        protected override void OnShow() {
            settings = SystemSettings.Settings;

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(GetResolutionOptionData());

            UpdateOptionValues(settings);
            ShowDisplayOptions();
        }

        protected override void OnClose() {
            SystemSettings.Save(SystemConstants.SavePath, SystemConstants.SettingsFileName);
        }

        #endregion

        #region Helper Methods

        private int GetResolutionIndex(int width, int height, int refreshRate) {
            Resolution[] availableResolutions = Screen.resolutions;
            int index = -1;

            for (int i = 0; i < availableResolutions.Length && index == -1; i++) {
                if (availableResolutions[i].width == width && availableResolutions[i].height == height && availableResolutions[i].refreshRate == refreshRate) {
                    index = i;
                }
            }

            return index;
        }

        private List<Dropdown.OptionData> GetResolutionOptionData() {
            Resolution[] availableResolutions = Screen.resolutions;
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>(availableResolutions.Length);

            for (int i = 0; i < availableResolutions.Length; i++) {
                options.Add(new Dropdown.OptionData(availableResolutions[i].ToString()));
            }

            return options;
        }

        #endregion
    }
}