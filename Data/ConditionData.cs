using System;
using UnityEngine;

namespace JusticeFramework.Data {
    [Serializable]
    public class ConditionData {
        /// <summary>
        /// The method used to evaluate the condition.
        /// </summary>
        [SerializeField]
        private EConditionType conditionType;

        /// <summary>
        /// The type of equality check to perform.
        /// </summary>
        [SerializeField]
        private EEqualityType equalityType;

        /// <summary>
        /// The value to compare against.
        /// </summary>
        [SerializeField]
        private string stringValue = "";

        /// <summary>
        /// The value to compare against.
        /// </summary>
        [SerializeField]
        private float floatValue = 1;

        /// <summary>
        /// Flag indicating if the condition should target self instead of the target.
        /// </summary>
        [SerializeField]
        private bool shouldTargetSelf = false;

        /// <summary>
        /// Gets the type of conditional check to perform.
        /// </summary>
        public EConditionType ConditionType {
            get { return conditionType; }
        }

        /// <summary>
        /// Gets the type of equality check to perform.
        /// </summary>
        public EEqualityType EqualityType {
            get { return equalityType; }
        }

        /// <summary>
        /// Gets the associated string value.
        /// </summary>
        public string StringValue {
            get { return stringValue; }
        }

        /// <summary>
        /// Gets the associated floating value.
        /// </summary>
        public float FloatValue {
            get { return floatValue; }
        }

        /// <summary>
        /// Gets wether this event should target the owner instead of the target. In most cases, "self" will be the player.
        /// </summary>
        public bool ShouldTargetSelf {
            get { return shouldTargetSelf; }
        }
    }
}
