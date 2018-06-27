namespace JusticeFramework.Core.Interfaces {
    public interface IInteractable : IWorldObject {
		EInteractionType InteractionType { get; }
	}
}