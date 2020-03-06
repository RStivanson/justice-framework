using JusticeFramework.Data;

namespace JusticeFramework.Interfaces {
    public interface ILockable {
		bool IsLocked { get; }
		ELockDifficulty LockDifficulty { get; }
        ItemData RequiredKey { get; }
	}
}