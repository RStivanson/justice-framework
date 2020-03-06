using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Setting object that contains data for in game settings
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Game Setting", order = 150)]
    public class GameSettingData : ScriptableDataObject {
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
        /// Gets the assigned float value.
        /// </summary>
        public float FloatValue {
            get { return floatValue; }
            set { floatValue = value; }
        }

        /// <summary>
        /// Gets the assigned string value.
        /// </summary>
        public string StringValue {
            get { return stringValue; }
            set { stringValue = value; }
        }

        /// <summary>
        /// Gets a flag indicating if this setting is using the float value
        /// </summary>
        public bool IsFloatSetting {
            get { return !IsStringSetting; }
        }

        /// <summary>
        /// Gets a flag indicating if this setting is using the string value
        /// </summary>
        public bool IsStringSetting {
            get { return stringValue.Length > 0; }
        }

        /// <summary>
        /// Gets the float value from this game setting.
        /// </summary>
        /// <param name="rhs">The game setting to convert</param>
        public static implicit operator float(GameSettingData rhs) {
            return rhs?.floatValue ?? 0;
        }

        /// <summary>
        /// Gets the string value from this game setting.
        /// </summary>
        /// <param name="rhs">The game setting to convert</param>
        public static implicit operator string(GameSettingData rhs) {
            return rhs?.stringValue;
        }
    }
}
