using UnityEngine;

namespace JusticeFramework.Core.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for weapons
	/// </summary>
	public interface IDoor : IWorldObject {
		/// <summary>
		/// Flag indicating if this door is currently locked
		/// </summary>
		bool IsLocked { get; }
		
		/// <summary>
		/// The difficulty of the lock on this door
		/// </summary>
		ELockDifficulty LockDifficulty { get; }
		
		/// <summary>
		/// The sound played when the door is opened
		/// </summary>
		AudioClip OpenSound { get; }
	}
}