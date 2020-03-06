using JusticeFramework.Interfaces;
using System;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Stores the data needed for a crafting recipe
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Recipe Data", order = 103)]
    public class RecipeData : ScriptableDataObject, IDisplayable {
        /// <summary>
        /// Defines the result from crafting a recipe
        /// </summary>
        [Serializable]
        public struct RecipeResult {
            /// <summary>
            /// The item to be given upon creation
            /// </summary>
            public ItemData itemData;

            /// <summary>
            /// The amount of the item to be given upon creation
            /// </summary>
            public int quantity;
        }

        /// <summary>
        /// Defines the ingredients needed for crafting a recipe
        /// </summary>
        [Serializable]
        public struct RecipeIngredient {
            /// <summary>
            /// The item required to be crafted
            /// </summary>
            public ItemData itemData;

            /// <summary>
            /// The amount of the item required to be crafted
            /// </summary>
            public int quantity;
        }

        /// <summary>
        /// The resulting item from the recipe
        /// </summary>
        [SerializeField]
        private RecipeResult resultItem;

        /// <summary>
        /// The required items to craft this recipe
        /// </summary>
        [SerializeField]
        private RecipeIngredient[] ingredients;

        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        public string DisplayName {
            get { return resultItem.itemData?.DisplayName; }
        }

        /// <summary>
        /// Gets the resulting item from this recipe
        /// </summary>
        public RecipeResult Result {
            get { return resultItem; }
        }

        /// <summary>
        /// Gets the ingredients for this recipe
        /// </summary>
        public RecipeIngredient[] Ingredients {
            get { return ingredients; }
        }
    }
}
