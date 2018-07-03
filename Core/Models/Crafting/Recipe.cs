using System;
using UnityEngine;

namespace JusticeFramework.Core.Models.Crafting {
    /// <inheritdoc />
    /// <summary>
    /// Model class for all crafting recipes
    /// </summary>
    [Serializable]
    public class Recipe : EntityModel {
        /// <summary>
        /// The resulting item from the recipe
        /// </summary>
        [SerializeField]
        public RecipeItem resultItem;

        /// <summary>
        /// The required items to craft this recipe
        /// </summary>
        [SerializeField]
        public RecipeItem[] ingredients;

        /// <summary>
        /// Gets the resulting item from this recipe
        /// </summary>
        public RecipeItem Result {
            get { return resultItem; }
        }

        /// <summary>
        /// Gets the ingredients for this recipe
        /// </summary>
        public RecipeItem[] Ingredients {
            get { return ingredients; }
        }
    }
}