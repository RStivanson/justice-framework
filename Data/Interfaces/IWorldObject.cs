using JusticeFramework.Data.Events;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	/// <summary>
	/// Base interface used for entity components that are attached to GameObjects
	/// </summary>
	public interface IWorldObject : IEntity {
		/// <summary>
		/// The transform attached to this object
		/// </summary>
		Transform Transform { get; }
		
		/// <summary>
		/// The name that is displayed in game
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Flag indicating if this object can be activated
		/// </summary>
		bool CanBeActivated { get; }

		/// <summary>
		/// Activates the object
		/// </summary>
		/// <param name="sender">The object sending the command</param>
		/// <param name="e">Event arguments attached to the activate command</param>
		void Activate(object sender, ActivateEventArgs e);
	}
}
