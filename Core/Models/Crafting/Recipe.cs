using JusticeFramework.Core.Collections;
using JusticeFramework.Core.Managers;
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

        #region Crafting Functions

        /// <summary>
        /// Attempts to craft the recipe
        /// </summary>
        /// <param name="recipeId">The ID of the recipe to craft</param>
        /// <param name="target">The target inventory to place the crafted item</param>
        /// <param name="source">The source inventory to get the ingredients from</param>
        /// <returns>Returns true if the recipe was crafted successfully, false otherwise</returns>
        public static bool Craft(string recipeId, Inventory target, Inventory source) {
            Recipe recipe = GameManager.RecipeManager.GetById(recipeId);
            return recipe?.Craft(target, source) ?? false;
        }

        /// <summary>
        /// Attempts to craft the recipe
        /// </summary>
        /// <param name="target">The target inventory to place the crafted item</param>
        /// <param name="source">The source inventory to get the ingredients from</param>
        /// <returns>Returns true if the recipe was crafted successfully, false otherwise</returns>
        public bool Craft(Inventory target, Inventory source) {
            bool crafted = false;

            // If the source has all the needed ingredients
            if (HasIngredients(source)) {
                // Remove the ingredients from the source inventory
                RemoveIngredients(source);

                // Add the result item to the target inventory
                target.Add(Result.Ingredient.id, Result.Quantity);
                crafted = true;
            }

            return crafted;
        }

        /// <summary>
        /// Determines if the inventory contains all the needed ingredients
        /// </summary>
        /// <param name="source">The inventory to check against</param>
        /// <returns>Returns true if the inventory has all the needed ingredients, false otherwise</returns>
        public bool HasIngredients(Inventory source) {
            bool hasIngredients = true;

            // For each ingredient
            foreach (RecipeItem recipeItem in Ingredients) {
                // Check if the inventory contains the ingredient
                hasIngredients &= source.Contains(recipeItem.Ingredient.id, recipeItem.Quantity);
            }

            return hasIngredients;
        }

        /// <summary>
        /// Removes the recipes ingredients from the inventory
        /// </summary>
        /// <param name="source">The inventory to remove the ingredients from</param>
        private void RemoveIngredients(Inventory source) {
            // For each ingredient, remove it
            foreach (RecipeItem recipeItem in Ingredients) {
                source.Remove(recipeItem.Ingredient.id, recipeItem.Quantity);
            }
        }

        #endregion
    }
}