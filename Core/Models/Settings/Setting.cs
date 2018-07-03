using JusticeFramework.Core.Models;
using System;
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
        /// Gets the float value of this setting
        /// </summary>
        public float FloatValue {
            get { return floatValue; }
        }

        /// <summary>
        /// Gets the string value of this setting
        /// </summary>
        public string StringValue {
            get { return stringValue; }
        }

        /// <summary>
        /// Gets a flag indicating if this setting is using the string or float value
        /// </summary>
        public bool IsStringSetting {
            get { return stringValue.Length > 0; }
        }
    }
}
