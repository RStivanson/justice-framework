using JusticeFramework.Components;

namespace JusticeFramework.Core.Interfaces {
    public delegate void OnWorldTargetChanged(WorldObject newTarget);

    public interface IInteractionController {
        event OnWorldTargetChanged onInteractionTargetChanged;

        WorldObject CurrentTarget { get; }
    }
}
