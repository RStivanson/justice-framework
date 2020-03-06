using JusticeFramework.Components;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionActivate : Action {
        private WorldObject[] linkedObjects;

        public ActionActivate(WorldObject target, WorldObject[] linkedObjects) : base(target) {
            this.linkedObjects = linkedObjects;
        }

        public ActionActivate(WorldObject target, WorldObject[] linkedObjects, AudioClip sound) : base(target, sound) {
            this.linkedObjects = linkedObjects;
        }

        public ActionActivate(WorldObject target, WorldObject[] linkedObjects, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
            this.linkedObjects = linkedObjects;
        }

        protected override void OnExecute(WorldObject actor) {
            Activator activator = target as Activator;
            activator.IsOn = !activator.IsOn;

            // Activate all linked objects
            foreach (WorldObject reference in linkedObjects) {
                Action action = reference?.Activate(activator);
                action.Execute(activator);
            }
        }
    }
}
