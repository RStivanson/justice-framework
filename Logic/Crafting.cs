using JusticeFramework.Collections;
using JusticeFramework.Data;
using JusticeFramework.Managers;

namespace JusticeFramework.Logic {
    /// <summary>
    /// Handles the logic relating to in-game recipes and crafting.
    /// </summary>
    public static class Crafting {
        /// <summary>
        /// Gets the displayed name.
        /// </summary>
        /// <param name="data">The recipe data</param>
        /// <returns>Returns the name of the recipe to display, null if it fails.</returns>
        public static string GetDisplayName(RecipeData data) {
            return data?.Result.itemData?.DisplayName;
        }

        /// <summary>
        /// Attempts to craft the recipe
        /// TODO: Crafting mechanics class? Mechanics class?
        /// </summary>
        /// <param name="recipeId">The ID of the recipe to craft</param>
        /// <param name="target">The target inventory to place the crafted item</param>
        /// <param name="source">The source inventory to get the ingredients from</param>
        /// <returns>Returns true if the recipe was crafted successfully, false otherwise</returns>
        public static bool Craft(string recipeId, Inventory target, Inventory source) {
            RecipeData data = GameManager.DataManager.GetRecipeById(recipeId);
            return Craft(data, target, source);
        }

        /// <summary>
        /// Attempts to craft the recipe
        /// TODO: Crafting mechanics class? Mechanics class?
        /// </summary>
        /// <param name="target">The target inventory to place the crafted item</param>
        /// <param name="source">The source inventory to get the ingredients from</param>
        /// <returns>Returns true if the recipe was crafted successfully, false otherwise</returns>
        public static bool Craft(RecipeData data, Inventory target, Inventory source) {
            bool crafted = false;

            // If the source has all the needed ingredients
            if (HasIngredients(data, source)) {
                // Remove the ingredients from the source inventory
                RemoveIngredients(data, source);

                // Add the result item to the target inventory
                target.Add(data.Result.itemData.Id, data.Result.quantity);
                crafted = true;
            }

            return crafted;
        }

        /// <summary>
        /// Determines if the inventory contains all the needed ingredients
        /// </summary>
        /// <param name="source">The inventory to check against</param>
        /// <returns>Returns true if the inventory has all the needed ingredients, false otherwise</returns>
        public static bool HasIngredients(RecipeData data, Inventory source) {
            bool hasIngredients = true;

            // For each ingredient
            foreach (RecipeData.RecipeIngredient ingredient in data.Ingredients) {
                // Check if the inventory contains the ingredient
                hasIngredients &= source.Contains(ingredient.itemData.Id, ingredient.quantity);
            }

            return hasIngredients;
        }

        /// <summary>
        /// Removes the recipes ingredients from the inventory
        /// </summary>
        /// <param name="source">The inventory to remove the ingredients from</param>
        private static void RemoveIngredients(RecipeData data, Inventory source) {
            // For each ingredient, remove it
            foreach (RecipeData.RecipeIngredient ingredient in data.Ingredients) {
                source.Remove(ingredient.itemData.Id, ingredient.quantity);
            }
        }
    }
}
