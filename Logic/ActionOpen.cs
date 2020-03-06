using JusticeFramework.Components;
using JusticeFramework.Interfaces;
using JusticeFramework.Managers;
using JusticeFramework.UI.Views;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionOpen : Action {
        public ActionOpen(WorldObject target) : base(target) {
        }

        public ActionOpen(WorldObject target, AudioClip sound) : base(target, sound) {
        }

        public ActionOpen(WorldObject target, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
        }

        protected override void OnExecute(WorldObject actor) {
            ContainerView view = UiManager.UI.OpenWindow<ContainerView>();
            view.View(actor as IContainer, target as IContainer);
        }
    }
}
