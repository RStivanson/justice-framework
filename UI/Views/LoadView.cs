using System;
using JusticeFramework.UI.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
	[Serializable]
	public class LoadView : Window {
		[SerializeField]
		private ProgressBar loadProgressBar;
		
		[SerializeField]
		private Text gameTipText;

		[SerializeField]
		[HideInInspector]
		private AsyncOperation asyncOperation = null;

		[SerializeField]
		private UnityAction onLoadFinish = null;
		
		[SerializeField]
		[HideInInspector]
		private bool isLoading = false;

		public bool IsLoading {
			get { return isLoading; }
		}

		public float Progress {
			get { return asyncOperation?.priority ?? 0;  }
		}
		
		private void Update() {
			if (asyncOperation == null || !isLoading) {
				return;
			}

			loadProgressBar.SetValue(asyncOperation.progress);

			if (!asyncOperation.isDone) {
				return;
			}
			
			onLoadFinish?.Invoke();
		}
		
		public void Monitor(AsyncOperation operation, UnityAction onFinishCallback) {
			asyncOperation = operation;
			onLoadFinish = onFinishCallback;

			isLoading = true;
		}

		public void SetGameTipText(string tip, bool show = true) {
			gameTipText.text = tip;
			gameTipText.gameObject.SetActive(show);
		}
	}
}