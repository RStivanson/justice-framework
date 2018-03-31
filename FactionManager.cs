using System;
using System.Collections.Generic;
using JusticeFramework.Data.Factions;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework {
	[Serializable]
	public class FactionManager : IOnProgressChanged {
		[SerializeField]
		private bool initialized = false;

		[SerializeField]
		private Faction[] factions;

		[SerializeField]
		private Dictionary<string, Faction> factionsById;

		public event OnProgressChanged onProgressChanged;

		public void Initialize() {
			if (initialized) {
				return;
			}

			initialized = true;

			if (onProgressChanged != null) {
				onProgressChanged(false, 0, "Loading actor factions");
			}

			factions = Resources.LoadAll<Faction>("Model/Factions/");
			factionsById = new Dictionary<string, Faction>();
			
			for (int i = 0; i < factions.Length; ++i) {
				if (onProgressChanged != null) {
					onProgressChanged(false, 0.2f + (0.8f * (i / (float)factions.Length)), "Post-processing faction: " + factions[i].id);
				}
				
				factionsById.Add(factions[i].id, factions[i]);
			}

			if (onProgressChanged != null) {
				onProgressChanged(true, 1.0f, "Done");
			}
		}
		
		private Faction GetFactionById(string id) {
			Faction faction;

			factionsById.TryGetValue(id, out faction);

			return faction;
		}
		
		public FactionAffinity GetAffinityBetween(string firstId, string secondId) {
			return GetFactionById(firstId).GetAffinity(secondId);
		}
	}
}