using System;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.Core.UI.Components {
    [Serializable]
    public class NotificationText : MonoBehaviour {
        [SerializeField]
        private float displayTime = 2.25f;

        [SerializeField]
        private Text buttonText;

        private float currentTimeDisplayed = 0;

        private void Update() {
            currentTimeDisplayed += Time.deltaTime;

            if (currentTimeDisplayed >= displayTime) {
                Destroy(gameObject);
            }
        }

        public void SetText(string text) {
            buttonText.text = text;
        }
    }
}
