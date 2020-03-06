using JusticeFramework.Core.Extensions;
using JusticeFramework.Data;

namespace JusticeFramework.Logic {
    public static class Faction {
        /// <summary>
        /// Determines if the given ID is assigned to this faction.
        /// </summary>
        /// <param name="data">The faction being checked</param>
        /// <param name="entityId">The entity to check against</param>
        /// <returns>Returns true if the entity is a member of the faction, false otherwise.</returns>
        public static bool IsMember(FactionData data, string entityId) {
            bool isMember = false;

            for (int i = 0; i < data.Members.Length && !isMember; i++) {
                isMember = data.Members[i].Id.EqualsOrdinal(entityId);
            }

            return isMember;
        }

        /// <summary>
        /// Gets the affinity towards the target factions
        /// </summary>
        /// <param name="data">The faction being checked</param>
        /// <param name="factionId">The faction to check against</param>
        /// <returns>Returns the affinity towards the other faction.</returns>
        public static EFactionAffinity GetAffinity(FactionData data, string factionId) {
            EFactionAffinity affinity = EFactionAffinity.Neutral;

            foreach (FactionData.FactionRelation relation in data.Relations) {
                if (relation.faction.Id.EqualsOrdinal(factionId)) {
                    affinity = relation.affinity;
                    break;
                }
            }

            return EFactionAffinity.Neutral;
        }

        /// <summary>
        /// Gets the affinity towards the target factions
        /// </summary>
        /// <param name="data">The faction being checked</param>
        /// <param name="factionId">The faction to check against</param>
        /// <returns>Returns the affinity towards the other faction.</returns>
        public static EFactionAffinity GetAffinity(FactionData data, FactionData otherFaction) {
            return GetAffinity(data, otherFaction.Id);
        }
    }
}
