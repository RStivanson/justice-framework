using JusticeFramework.Components;
using JusticeFramework.Managers;
using JusticeFramework.UI.Views;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionRead : Action {
        public ActionRead(WorldObject target) : base(target) {
        }

        public ActionRead(WorldObject target, AudioClip sound) : base(target, sound) {
        }

        public ActionRead(WorldObject target, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
        }

        protected override void OnExecute(WorldObject actor) {
            CodexView view = UiManager.UI.OpenWindow<CodexView>();
            view.SetCodex(target as Components.Codex);
        }
    }
}
