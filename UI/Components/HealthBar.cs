using JusticeFramework.Interfaces;
using System;

namespace JusticeFramework.UI.Components {
    [Serializable]
	public class HealthBar : ProgressBar {
		private IDamageable toMonitor;

		public void Monitor(IDamageable damageable) {
			if (toMonitor != null) {
				toMonitor.onCurrentHealthChanged -= OnMonitorHealthChanged;
			}

			toMonitor = damageable;

			if (toMonitor == null) {
				return;
			}

			toMonitor.onCurrentHealthChanged += OnMonitorHealthChanged;
			OnMonitorHealthChanged(toMonitor);
		}

		private void OnMonitorHealthChanged(IDamageable damageable) {
			if (damageable != null) {
				SetValue(damageable.CurrentHealth, damageable.MaxHealth);
			}
		}

		protected override void OnClose() {
			if (toMonitor != null) {
				toMonitor.onCurrentHealthChanged -= OnMonitorHealthChanged;
			}
		}
	}
}