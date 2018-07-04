using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace JusticeFramework.Core.Models.Settings {
    /// <summary>
    /// Data class that holds information pertaining to a game setting
    /// </summary>
    [Serializable]
    public class Setting : EntityModel {
        /// <summary>
        /// Floating numeric value represented by this setting
        /// </summary>
        [SerializeField]
        private float floatValue;

        /// <summary>
        /// String value represented by this setting
        /// </summary>
        [SerializeField]
        private string stringValue;

        /// <summary>
        /// Flag indicating if this setting is a system setting
        /// </summary>
        [SerializeField]
        [HideInInspector]
        private bool isSystemSetting;

        /// <summary>
        /// Gets the float value of this setting
        /// </summary>
        public float FloatValue {
            get { return floatValue; }
            set { floatValue = value; }
        }

        /// <summary>
        /// Gets the string value of this setting
        /// </summary>
        public string StringValue {
            get { return stringValue; }
            set { stringValue = value; }
        }

        /// <summary>
        /// Gets a flag indicating if this setting is using the string or float value
        /// </summary>
        public bool IsStringSetting {
            get { return stringValue.Length > 0; }
        }

        /// <summary>
        /// Gets a flag indicating if this setting is considered a system setting
        /// </summary>
        public bool IsSystemSetting {
            get { return isSystemSetting; }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Sets the system setting flag to true
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("CONTEXT/Setting/Toggle System Setting Flag")]
        public static void SetAsSystemSetting(MenuCommand menuCommand) {
            Setting setting = menuCommand.context as Setting;

            if (setting != null) {
                setting.isSystemSetting = !setting.isSystemSetting;
            }
        }
#endif
    }
}
