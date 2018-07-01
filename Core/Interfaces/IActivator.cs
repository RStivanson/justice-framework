using JusticeFramework.Core.Events;
using UnityEngine;

namespace JusticeFramework.Core.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for activators, levers, switches, etc
	/// </summary>
	public interface IActivator : IWorldObject {
		/// <summary>
		/// Event called when the activator is activated
		/// </summary>
		event OnActivationStateChanged OnActivated;
		
		/// <summary>
		/// The sound played when the activator is activated
		/// </summary>
		AudioClip ActivationSound { get; set; }
	}
}