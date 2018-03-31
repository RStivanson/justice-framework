using System;
using JusticeFramework.Components;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
	[Serializable]
	public class CodexView : Window {
		[SerializeField]
		private Text title;

		[SerializeField]
		private Text content;
		
		[SerializeField]
		[HideInInspector]
		private Codex current;

		public Codex Current {
			get { return current; }
		}

		private void Awake() {
			Clear();
		}
		
		public void SetCodex(Codex codex) {
			current = codex;

			if (current == null) {
				title.text = string.Empty;
				content.text = string.Empty;
			} else {
				title.text = current.DisplayName;
				content.text = current.Text.Replace("<br>", "\n");
			}
		}

		public void Clear() {
			current = null;
			title.text = string.Empty;
			content.text = string.Empty;
		}
	}
}