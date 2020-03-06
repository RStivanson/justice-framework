using JusticeFramework.Components;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionTake : Action {
        public ActionTake(WorldObject target) : base(target) {
        }

        public ActionTake(WorldObject target, AudioClip sound) : base(target, sound) {
        }

        public ActionTake(WorldObject target, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
        }

        protected override void OnExecute(WorldObject actor) {
            ItemData data = target.GetData<ItemData>();
            IContainer container = actor as IContainer;

            // TODO
            Debug.LogWarning("Stack amount given 1");
            container.Inventory.Add(data.Id, 1);
            Mechanics.OnItemTake(actor, target, data, 1);

            target.Destroy();
        }
    }
}
