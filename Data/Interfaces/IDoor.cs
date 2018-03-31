using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	public interface IDoor : IWorldObject {
		bool IsLocked { get; }
		ELockDifficulty LockDifficulty { get; }
		EDoorType DoorType { get; }
		string DestinationScene { get; }
		Vector3 DestinationPosition { get; }
		AudioClip OpenSound { get; }
	}
}