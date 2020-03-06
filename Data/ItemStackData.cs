using System;

namespace JusticeFramework.Data {
    /// <summary>
    /// Describes an item and a quantity
    /// </summary>
    [Serializable]
    public class ItemStackData {
        public ItemData itemData;
        public int quantity = 1;
    }
}
