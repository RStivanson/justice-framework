using UnityEngine;

namespace JusticeFramework.Core {
    /// <summary>
    /// Attribute that marks an array or list to be drawn using the reorderable list UI component.
    /// </summary>
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class ReorderableAttribute : PropertyAttribute {
        public bool Draggable {
            get; private set;
        }

        public bool DisplayHeader {
            get; private set;
        }

        public bool DisplayAddButton {
            get; private set;
        }

        public bool DisplayRemoveButton {
            get; private set;
        }

        public ReorderableAttribute() : this(true, true, true, true) {
        }

        public ReorderableAttribute(bool draggable, bool displayHeader, bool displayAddButton, bool displayRemoveButton) {
            Draggable = draggable;
            DisplayHeader = displayHeader;
            DisplayAddButton = displayAddButton;
            DisplayRemoveButton = displayRemoveButton;
        }
    }
}
