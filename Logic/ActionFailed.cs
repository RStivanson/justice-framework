using JusticeFramework.Components;
using JusticeFramework.Managers;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionFailed : Action {
        private string message;

        public ActionFailed(WorldObject target, string message) : base(target) {
            this.message = message;
        }

        public ActionFailed(WorldObject target, string message, AudioClip sound) : base(target, sound) {
            this.message = message;
        }

        public ActionFailed(WorldObject target, string message, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
            this.message = message;
        }

        protected override void OnExecute(WorldObject actor) {
            UiManager.Notify(message);
        }
    }
}
