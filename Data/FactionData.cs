using System;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Stores information regarding a faction
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Faction Data", order = 101)]
    public class FactionData : ScriptableDataObject {
        /// <summary>
        /// Container that stores a relationship between this and another faction
        /// </summary>
        [Serializable]
        public struct FactionRelation {
            public FactionData faction;
            public EFactionAffinity affinity;
        }

        /// <summary>
        /// A list of actors that are apart of this faction.
        /// </summary>
        [SerializeField]
        private ActorData[] members;

        /// <summary>
        /// A list of relations that this faction has with other factions.
        /// </summary>
        [SerializeField]
        private FactionRelation[] relations;

        /// <summary>
        /// Gets the list of members of this faction.
        /// </summary>
        public ActorData[] Members {
            get { return members; }
        }

        /// <summary>
        /// Gets the list of all relations this faction has with other factions.
        /// </summary>
        public FactionRelation[] Relations {
            get { return relations; }
        }
    }
}