using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Data.Factions {
	/// <summary>
	/// Stores information regarding a faction
	/// </summary>
	[Serializable]
	public class Faction : ScriptableObject {
		/// <summary>
		/// Unique identifier that distinguishes the faction
		/// </summary>
		[SerializeField]
		private string id;
		
		/// <summary>
		/// A list of actors that are apart of this faction
		/// </summary>
		[SerializeField]
		private List<string> memberIds;
		
		/// <summary>
		/// A list of relations that this faction has for other factions
		/// </summary>
		[SerializeField]
		private List<FactionRelation> relations;

		/// <summary>
		/// Gets the Id of this faction
		/// </summary>
		public string Id {
			get { return id; }
		}

		/// <summary>
		/// Gets the members of this faction
		/// </summary>
		public List<string> MemberIds {
			get { return memberIds; }
		}

		/// <summary>
		/// Gets the relations of this factions
		/// </summary>
		public List<FactionRelation> Relations {
			get { return relations; }
		}
		
		/// <summary>
		/// Gets the affinity towards the target factions
		/// </summary>
		/// <param name="factionId"></param>
		/// <returns></returns>
		public FactionAffinity GetAffinity(string factionId) {
			FactionAffinity affinity = FactionAffinity.Nuetral;
			
			// For each relation, while we haven't found the target
			foreach (FactionRelation relation in relations) {
				// If this is not the faction we are looking for, skip
				if (!relation.FactionId.Equals(factionId)) {
					continue;
				}

				affinity = relation.Affinity;
				break;
			}

			return affinity;
		}
	}
}