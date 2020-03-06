using JusticeFramework.Components;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Managers;
using JusticeFramework.UI.Views;

namespace JusticeFramework.Logic {
    public class ActionSpawnAtPlayer : Action {
        private ItemData itemData;
        private int quantity;

        public ActionSpawnAtPlayer(WorldObject target, ItemData itemData, int quantity) : base(target) {
            this.itemData = itemData;
            this.quantity = quantity;
        }

        protected override void OnExecute(WorldObject actor) {
            IItem item = GameManager.SpawnAtActor(itemData, GameManager.GetPlayer()) as IItem;

            if (item != null) {
                item.StackAmount = quantity;
            }
        }
    }
}
