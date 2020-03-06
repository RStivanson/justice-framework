using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Setting object that links engine keys to data
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Engine Object", order = 154)]
    public class EngineObjectData : ScriptableDataObject {
        /// <summary>
        /// Object value represented by this setting
        /// </summary>
        [SerializeField]
        private Object objectValue;

        /// <summary>
        /// Gets the assigned string value.
        /// </summary>
        public Object ObjectValue {
            get { return objectValue; }
            set { objectValue = value; }
        }
    }
}
