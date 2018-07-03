using System;
using JusticeFramework.Core.Interfaces;
using JusticeFramework.UI.Components;
using JusticeFramework.Utility.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using JusticeFramework.Core.UI;
using JusticeFramework.Core.Managers;
using JusticeFramework.Core.Models.Crafting;

namespace JusticeFramework.UI.Views {
    [Serializable]
    public class CraftingView : Window {
        private const KeyCode CRAFT_KEY_CODE = KeyCode.E;

        [SerializeField]
        private Text selectedRecipeNameLabel;

        [SerializeField]
        private Text selectedRecipeDescrLabel;

        [SerializeField]
        private GameObject recipeButtonPrefab;

        [SerializeField]
        private Transform recipeContainer;

        private IContainer ingredientContainer;
        private int selectedRecipeIndex;
        private Recipe selectedRecipe;

        #region Unity Events

        private void Update() {
            // If we do not have any item selected, return
            if (selectedRecipe == null) {
                return;
            }

            if (Input.GetKeyDown(CRAFT_KEY_CODE) && HasIngredients(ingredientContainer, selectedRecipe)) {
                OnCraftingRecipeActivated(ingredientContainer, selectedRecipe);
            }
        }

        #endregion

        #region Event Callbacks

        private void OnRecipeSelected(int index, Recipe recipe) {
            selectedRecipeIndex = index;
            selectedRecipe = recipe;

            selectedRecipeNameLabel.text = recipe.Result.Ingredient.displayName;
            selectedRecipeDescrLabel.text = string.Empty;

            foreach (RecipeItem recipeItem in recipe.Ingredients) {
                selectedRecipeDescrLabel.text += $"{recipeItem.Ingredient.displayName} ({recipeItem.Quantity}){Environment.NewLine}";
            }
        }

        #endregion

        private bool HasIngredients(IContainer ingredientContainer, Recipe recipe) {
            bool hasIngredients = true;
            int containerQuantity;

            foreach (RecipeItem recipeItem in recipe.Ingredients) {
                containerQuantity = ingredientContainer.GetQuantity(recipeItem.Ingredient.id);
                hasIngredients = hasIngredients && containerQuantity >= recipeItem.Quantity;
            }

            return hasIngredients;
        }

        public static void OnCraftingRecipeActivated(IContainer container, Recipe recipe) {
            foreach (RecipeItem recipeItem in recipe.Ingredients) {
                container.TakeItem(recipeItem.Ingredient.id, recipeItem.Quantity);
            }

            container.GiveItem(recipe.Result.Ingredient.id, recipe.Result.Quantity);
        }

        private void RefreshRecipeList() {
            bool entryFound = false;
            Recipe[] recipes = GameManager.RecipeManager.GetAll();
            recipeContainer.DestroyAllChildren();

            for (int i = 0; i < recipes.Length; ++i) {
                int index = i;
                Recipe recipe = recipes[i];

                if (recipe == null) {
                    continue;
                }

                if (selectedRecipe != null && ReferenceEquals(recipe, selectedRecipe)) {
                    OnRecipeSelected(index, recipe);
                    entryFound = true;
                }

                AddButton(recipeContainer, recipe, delegate {
                    OnRecipeSelected(index, recipe);
                });
            }

            if (!entryFound) {
                ClearSelectedItem();
            }
        }

        private void AddButton(Transform container, Recipe recipe, UnityAction callback) {
            GameObject spawnedObject = Instantiate(recipeButtonPrefab);

            spawnedObject.GetComponent<Button>().onClick.AddListener(callback);
            spawnedObject.GetComponent<RecipeListButton>().SetRecipe(recipe);

            spawnedObject.transform.SetParent(container, false);
        }

        private void ClearSelectedItem() {
            selectedRecipeIndex = -1;
            selectedRecipe = null;

            selectedRecipeNameLabel.text = string.Empty;
            selectedRecipeDescrLabel.enabled = false;
        }

        public void View(IContainer ingredientContainer) {
            if (ingredientContainer == null) {
                return;
            }

            this.ingredientContainer = ingredientContainer;

            RefreshRecipeList();

            ClearSelectedItem();
            Show();
        }
    }
}