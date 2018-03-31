using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Data.Factions {
	[Serializable]
	public class Faction : ScriptableObject {
		public string id;
		public List<string> memberIds;
		public List<FactionRelation> relations;
		
		public FactionAffinity GetAffinity(string factionId) {
			FactionAffinity affinity = FactionAffinity.Nuetral;

			for (int i = 0; i < relations.Count; ++i) {
				if (relations[i].id.Equals(factionId)) {
					affinity = relations[i].affinity;
					break;
				}
			}

			return affinity;
		}
	}
}