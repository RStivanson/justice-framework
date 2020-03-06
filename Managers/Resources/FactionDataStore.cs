using JusticeFramework.Core;
using JusticeFramework.Data;
using JusticeFramework.Logic;
using System;
using System.Collections.Generic;

namespace JusticeFramework.Managers {
    [Serializable]
    public class FactionDataStore : ResourceStore<FactionData> {
        public List<FactionData> GetFactionsWithMember(string id) {
            List<FactionData> results = new List<FactionData>();

            foreach (FactionData factionData in resources) {
                if (Faction.IsMember(factionData, id)) {
                    results.Add(factionData);
                }
            }

            return results;
        }
    }
}