using JusticeFramework.Data.Models;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	public interface IFlower : IWorldObject {
		bool CanBeHarvested { get; }
		string GivesId { get; }
		int GivesQuantity { get; }
		int RespawnTimeInSeconds { get; }
		AudioClip HarvestSound { get; }

		void Harvest(IContainer container);
	}
}