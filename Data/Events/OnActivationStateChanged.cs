using JusticeFramework.Data.Interfaces;

namespace JusticeFramework.Data.Events {
	public delegate void OnActivationStateChanged(IActivator activator, object activatedBy, bool isOn);
}