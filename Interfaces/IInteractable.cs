namespace JusticeFramework.Interfaces {
    public interface IInteractable : IWorldObject {
		EInteractionType InteractionType { get; }
	}
}