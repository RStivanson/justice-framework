using JusticeFramework.Core.Interfaces;

namespace JusticeFramework.Core.Events {
	public delegate void OnActivationStateChanged(IActivator activator, object activatedBy, bool isOn);
}