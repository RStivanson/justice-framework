using JusticeFramework.Components;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionHarvest : Action {
        public ActionHarvest(WorldObject target) : base(target) {
        }

        public ActionHarvest(WorldObject target, AudioClip sound) : base(target, sound) {
        }

        public ActionHarvest(WorldObject target, AudioClip sound, EAudioType audioType, float volume) : base(target, sound, audioType, volume) {
        }

        protected override void OnExecute(WorldObject actor) {
            HarvestableData data = target.GetData<HarvestableData>();
            IContainer container = actor as IContainer;

            container.Inventory.Add(data.HarvestOutput.itemData.Id, data.HarvestOutput.quantity);
        }
    }
}
