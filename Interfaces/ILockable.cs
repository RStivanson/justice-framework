using JusticeFramework.Core;

namespace JusticeFramework.Interfaces {
	public interface ILockable {
		bool IsLocked { get; }
		ELockDifficulty LockDifficulty { get; }
	}
}