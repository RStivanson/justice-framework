using UnityEngine;

namespace JusticeFramework.Core {
    /// <summary>
    /// Attribute that specifies that a field should be drawn as read only/disabled in the inspector.
    /// </summary>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class ReadOnlyInInspectorAttribute : PropertyAttribute {
	}
}