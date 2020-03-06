using JusticeFramework.Components;
using JusticeFramework.Data;
using JusticeFramework.Managers;

namespace JusticeFramework.Logic {
    public static class Mechanics {

        public static void OnItemTake(WorldObject taker, WorldObject source, ItemData item, int quantity) {
            if (GameManager.IsPlayer(taker)) {
                UiManager.Notify($"Received item {item?.DisplayName}{(quantity > 1 ? $" ({quantity})" : string.Empty)}");
            }
        }

        public static void OnItemDrop(WorldObject source, ItemData item, int quantity) {
            if (GameManager.IsPlayer(source)) {
                UiManager.Notify($"Removed item {item?.DisplayName}{(quantity > 1 ? $" ({quantity})" : string.Empty)}");
            }
        }
    }
}
