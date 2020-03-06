using System;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Components {
    [Serializable]
    public class NotificationText : MonoBehaviour {
        [SerializeField]
        private float displayTime = 3.25f;

        [SerializeField]
        private Text buttonText;

        [SerializeField]
        private float fadeSpeed = 2.25f;

        private float currentTimeDisplayed = 0;

        private void Update() {
            currentTimeDisplayed += Time.unscaledDeltaTime;

            if (currentTimeDisplayed >= displayTime) {
                if (buttonText.color != Color.clear) {
                    buttonText.color = Color.Lerp(buttonText.color, Color.clear, Time.unscaledDeltaTime * fadeSpeed);
                } else {
                    Destroy(gameObject);
                }
            }
        }

        public void SetText(string text) {
            buttonText.text = text;
        }
    }
}
