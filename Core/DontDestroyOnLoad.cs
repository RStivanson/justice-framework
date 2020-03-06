using UnityEngine;

namespace JusticeFramework.Core
{
    /// <summary>
    /// Marks the current object to not be destroyed on load.
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        /// <summary>
        /// Used for initialization.
        /// </summary>
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}
