using JusticeFramework.Components;
using JusticeFramework.Managers;
using JusticeFramework.UI.Views;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionTalk : Action {
        public ActionTalk(WorldObject target) : base(target) {
        }

        public ActionTalk(WorldObject target, AudioClip sound) : base(target, sound) {
        }

        public ActionTalk(WorldObject target, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
        }

        protected override void OnExecute(WorldObject actor) {
            DialogueView view = UiManager.UI.OpenWindow<DialogueView>();
            view.SetTarget(target as Components.Actor);
        }
    }
}
