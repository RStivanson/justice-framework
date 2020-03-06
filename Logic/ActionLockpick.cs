using JusticeFramework.Components;
using System;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionLockpick : Action {
        private string message;

        public ActionLockpick(WorldObject target) : base(target) {
        }

        public ActionLockpick(WorldObject target, AudioClip sound) : base(target, sound) {
        }

        public ActionLockpick(WorldObject target, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
        }

        protected override void OnExecute(WorldObject actor) {
            throw new NotImplementedException();
        }
    }
}
