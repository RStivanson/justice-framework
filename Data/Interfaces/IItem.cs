using JusticeFramework.Data.Events;
using UnityEngine;

namespace JusticeFramework.Data.Interfaces {
	public interface IItem : IWorldObject {
		float Weight { get; }
		int Value { get; }
		
		bool IsStackable { get; }
		int MaxStackAmount { get; }
		
		AudioClip PickupSound { get; }
		AudioClip DropSound { get; }
	}
}