using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	/// <inheritdoc cref="IContainer" />
	/// <summary>
	/// Interface that defines attributes needed for chests, footlockers, etc
	/// </summary>
	public interface IChest : IWorldObject, IContainer {
		/// <summary>
		/// Flag indicating if this chest is locked
		/// </summary>
		bool IsLocked { get; }
		
		/// <summary>
		/// The difficulty of the lock on this chest
		/// </summary>
		ELockDifficulty LockDifficulty { get; }
		
		/// <summary>
		/// The sound played when the chest is opened
		/// </summary>
		AudioClip OpenSound { get; }
	}
}