using System;
using UnityEngine;

namespace JusticeFramework.Data.Factions {
	/// <summary>
	/// Relation definition on how one faction feels about another
	/// </summary>
	[Serializable]
	public class FactionRelation {
		/// <summary>
		/// The Id of the faction
		/// </summary>
		[SerializeField]
		private string factionId;
		
		/// <summary>
		/// The relationship affinity towards the faction
		/// </summary>
		[SerializeField]
		private FactionAffinity affinity;
		
		/// <summary>
		/// Gets the faction Id in this relation
		/// </summary>
		public string FactionId {
			get { return factionId; }
		}

		/// <summary>
		/// Gets the affinity in this relation
		/// </summary>
		public FactionAffinity Affinity {
			get { return affinity; }
		}
	}
}