using UnityEngine;

namespace JusticeFramework.Core.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for harvestable objects
	/// </summary>
	public interface IFlower : IWorldObject {
		/// <summary>
		/// Flag indicating if this object can be harvested
		/// </summary>
		bool CanBeHarvested { get; }
		
		/// <summary>
		/// The amount of time needed for this harvestable to respawn
		/// </summary>
		int RespawnTimeInSeconds { get; }
		
		/// <summary>
		/// The sound played when the object is harvested
		/// </summary>
		AudioClip HarvestSound { get; }

		/// <summary>
		/// Harvests the object and gives its item to the container
		/// </summary>
		/// <param name="container">The container that should have its item added to</param>
		void Harvest(IContainer container);
	}
}