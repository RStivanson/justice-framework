using System;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Defines a section of dialogue including the NPCs dialogue and the player's responses.
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Dialogue Data", order = 100)]
    [Serializable]
    public class DialogueData : ScriptableDataObject {
        /// <summary>
        /// The actor this dialogue targets and is displayed when talking to.
        /// </summary>
        [SerializeField]
        private ActorData targetActor;

        /// <summary>
        /// The faction this dialogue targets and is displayed when talking to.
        /// </summary>
        [SerializeField]
        private FactionData targetFaction;

        /// <summary>
        /// The collection of topics in this dialogue set.
        /// </summary>
        [SerializeField]
        private DialogueTopicData[] topics;

        /// <summary>
        /// Gets the actor that this dialogue should diplay for.
        /// </summary>
        public ActorData TargetActor {
            get { return targetActor; }
        }

        /// <summary>
        /// Gets the faction that this dialogue should display for.
        /// </summary>
        public FactionData TargetFaction {
            get { return targetFaction; }
        }

        /// <summary>
        /// Gets the topics in this dialogue set.
        /// </summary>
        public DialogueTopicData[] Topics {
            get { return topics; }
        }
    }
}