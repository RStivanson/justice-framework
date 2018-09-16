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
using JusticeFramework.Core.Collections;

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

            if (Input.GetKeyDown(CRAFT_KEY_CODE)) {
                OnItemActivated(GameManager.Player, ingredientContainer, selectedRecipe);
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

        public void OnItemActivated(IContainer target, IContainer source, Recipe recipe) {
            if (CraftRecipe(target.Inventory, source.Inventory, recipe)) {
                // Update ingredient display?
            }
        }

        #endregion

        #region Crafting Functions

        private bool CraftRecipe(Inventory target, Inventory source, Recipe recipe) {
            bool crafted = false;

            if (HasIngredients(source, recipe)) {
                target.Add(recipe.Result.Ingredient.id, recipe.Result.Quantity);
                RemoveIngredients(source, recipe);
                crafted = true;
            }

            return crafted;
        }

        private bool HasIngredients(Inventory source, Recipe recipe) {
            bool hasIngredients = true;

            foreach (RecipeItem recipeItem in recipe.Ingredients) {
                hasIngredients &= source.Contains(recipeItem.Ingredient.id, recipeItem.Quantity);
            }

            return hasIngredients;
        }

        private void RemoveIngredients(Inventory source, Recipe recipe) {
            foreach (RecipeItem recipeItem in recipe.Ingredients) {
                source.Remove(recipeItem.Ingredient.id, recipeItem.Quantity);
            }
        }

        #endregion

        #region UI Functions

        private void RefreshRecipeList() {
            Recipe[] recipes = GameManager.RecipeManager.Resources;
            bool entryFound = false;

            recipeContainer.DestroyAllChildren();

            for (int i = 0; i < recipes.Length; ++i) {
                int index = i;
                Recipe recipe = recipes[i];

                if (recipe == null) {
                    continue;
                }

                AddButton(recipeContainer, recipe, delegate {
                    OnRecipeSelected(index, recipe);
                });

                if (!entryFound && selectedRecipe != null && ReferenceEquals(recipe, selectedRecipe)) {
                    OnRecipeSelected(index, recipe);
                    entryFound = true;
                }
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
            selectedRecipe = null;

            selectedRecipeNameLabel.text = string.Empty;
            selectedRecipeDescrLabel.text = string.Empty;
        }

        #endregion

        public void View(IContainer container) {
            if (container == null) {
                Debug.Log($"Ingredient container is NULL");
                return;
            }

            ingredientContainer = container;

            RefreshRecipeList();
            ClearSelectedItem();
            Show();
        }
    }
}