using JusticeFramework.Data.Events;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	public interface IWorldObject : IEntity {
		Transform Transform { get; }
		string DisplayName { get; }
		bool CanBeActivated { get; }

		void Activate(object sender, ActivateEventArgs e);
	}
}
