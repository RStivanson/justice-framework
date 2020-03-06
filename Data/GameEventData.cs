using System;
using UnityEngine;

namespace JusticeFramework.Data {
    [Serializable]
    public class GameEventData {
        /// <summary>
        /// The type of event this data represents.
        /// </summary>
        [SerializeField]
        private EGameEventType gameEventType;

        /// <summary>
        /// The target ID of this event.
        /// </summary>
        [SerializeField]
        private string targetId;

        /// <summary>
        /// The value assigned to this event.
        /// </summary>
        [SerializeField]
        private int value;

        /// <summary>
        /// Flag indicating if this event should be executed silently
        /// </summary>
        private bool isSilent;

        /// <summary>
        /// Flag indicating if this event should target the owner.
        /// </summary>
        [SerializeField]
        private bool shouldTargetSelf;

        /// <summary>
        /// Gets the event type associated with the event.
        /// </summary>
        public EGameEventType EventType {
            get { return gameEventType; }
        }

        /// <summary>
        /// Gets the target ID of this event.
        /// </summary>
        public string TargetId {
            get { return targetId; }
        }

        /// <summary>
        /// Gets the value associated with this event
        /// </summary>
        public int Value {
            get { return value; }
        }

        /// <summary>
        /// Gets wether this event be executed silently.
        /// </summary>
        public bool IsSilent {
            get { return isSilent; }
        }

        /// <summary>
        /// Gets wether this event should target the owner instead of the target. In most cases, "self" will be the player.
        /// </summary>
        public bool ShouldTargetSelf {
            get { return shouldTargetSelf; }
        }
    }
}
