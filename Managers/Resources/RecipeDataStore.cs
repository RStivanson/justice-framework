using JusticeFramework.Collections;
using JusticeFramework.Core;
using JusticeFramework.Data;
using JusticeFramework.Logic;
using System;
using System.Collections.Generic;

namespace JusticeFramework.Managers.Resources {
    [Serializable]
    public class RecipeDataStore : ResourceStore<RecipeData> {
        public List<RecipeData> GetAllWithMatchingIngredients(Inventory inventory) {
            List<RecipeData> results = new List<RecipeData>();

            foreach (RecipeData recipe in resources) {
                if (Crafting.HasIngredients(recipe, inventory)) {
                    results.Add(recipe);
                }
            }

            return results;
        }
    }
}
