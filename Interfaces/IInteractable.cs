using JusticeFramework.Data;
using JusticeFramework.Data.Interfaces;
using JusticeFramework.Data.Models;

namespace JusticeFramework.Interfaces {
	public interface IInteractable : IWorldObject {
		EInteractionType InteractionType { get; }
	}
}