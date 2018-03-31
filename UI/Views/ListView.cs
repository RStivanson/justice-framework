using System;
using System.Collections.Generic;
using JusticeFramework.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
	[Serializable]
	public class ListView : Window {
		[SerializeField]
		protected RectTransform buttonContainer;
		
		[SerializeField]
		protected GameObject buttonTemplate;

		[SerializeField]
		[HideInInspector]
		protected List<Transform> spawnedButtons;

		public int Count {
			get { return spawnedButtons.Count; }
		}

		private void Awake() {
			spawnedButtons = new List<Transform>();

			for (int i = 0; i < buttonContainer.childCount; i++) {
				spawnedButtons.Add(buttonContainer.GetChild(i));
			}
		}

		public void AddButton(string buttonText, UnityAction clickEvent) {
			Transform button = Instantiate(buttonTemplate).transform;
			button.name = buttonText;

			button.GetComponent<Button>().onClick.AddListener(clickEvent);
			button.GetComponentInChildren<Text>().text = buttonText;

			RegisterButton(button);
		}

		protected void RegisterButton(Transform button) {
			if (button == null) {
				return;
			}

			button.SetParent(buttonContainer, false);
			spawnedButtons.Add(button);
		}

		public void RemoveButton(int index) {
			if (index < 0 || index >= spawnedButtons.Count) {
				return;
			}

			Destroy(spawnedButtons[index]);
			spawnedButtons.RemoveAt(index);
		}

		public void ClearButtons() {
			if (spawnedButtons == null) {
				spawnedButtons = new List<Transform>();
				return;
			}

			foreach (Transform listButton in spawnedButtons) {
				Destroy(listButton.gameObject);
			}

			spawnedButtons = new List<Transform>();
		}
	}
}
