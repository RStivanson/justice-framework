namespace JusticeFramework {
	public class InteractionInfo {
		public readonly string name;
		public readonly EInteractionType interactionType;

		public InteractionInfo(string name, EInteractionType interactionType) {
			this.name = name;
			this.interactionType = interactionType;
		}

		public override string ToString() {
			return (name + "\n" + (interactionType == EInteractionType.None ? string.Empty : interactionType.ToString()));
		}
	}
}