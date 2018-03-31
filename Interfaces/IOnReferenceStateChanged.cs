using JusticeFramework.Components;

namespace JusticeFramework.Interfaces {
	public delegate void OnReferenceStateChanged(Reference changed);

	public interface IOnReferenceStateChanged {
		event OnReferenceStateChanged onReferenceStateChanged;
	}
}