using JusticeFramework.Data.Events;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	public interface IActivator : IWorldObject {
		event OnActivationStateChanged OnActivated;
		
		GameObject[] Linked { get; set; }
		AudioClip ActivationSound { get; set; }
	}
}