using JusticeFramework.Core.UI;
using JusticeFramework.UI.Components;
using UnityEngine;

namespace JusticeFramework.UI.Views {
    public class NotificationView : Window {
        [SerializeField]
        private Transform displayContainer;

        [SerializeField]
        private GameObject notificationPrefab;

        public void CreateNotification(string displayText) {
            GameObject newNotif = Instantiate(notificationPrefab, displayContainer, false);

            newNotif.GetComponent<NotificationText>().SetText(displayText);
        }
    }
}
