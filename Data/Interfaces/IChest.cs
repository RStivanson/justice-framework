using JusticeFramework.Data.Collections;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	public interface IChest : IWorldObject, IContainer {
		bool IsLocked { get; }
		ELockDifficulty LockDifficulty { get; }
		AudioClip OpenSound { get; }
	}
}