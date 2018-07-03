using System;
using JusticeFramework.Core.Models.Crafting;

namespace JusticeFramework.Core.Managers.Resources {
    [Serializable]
    public class RecipeManager : ResourceManager<Recipe> {
        private string DataPath = "Data/Recipes";

        public override void LoadResources() {
            LoadResources(DataPath);
        }
    }
}
