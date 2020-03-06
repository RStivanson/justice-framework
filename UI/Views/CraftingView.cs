using JusticeFramework.Core.Extensions;
using JusticeFramework.Core.UI;
using JusticeFramework.Data;
using JusticeFramework.Interfaces;
using JusticeFramework.Logic;
using JusticeFramework.Managers;
using JusticeFramework.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JusticeFramework.UI.Views {
    [Serializable]
    public class CraftingView : Window {
        private const KeyCode CraftKeyCode = KeyCode.E;

        [SerializeField]
        private Text selectedRecipeNameLabel;

        [SerializeField]
        private Text selectedRecipeDescrLabel;

        [SerializeField]
        private Transform recipeContainer;

        [SerializeField]
        private GameObject recipeButtonPrefab;

        [SerializeField]
        private GameObject categoryHeaderPrefab;

        private IContainer ingredientContainer;
        private List<RecipeData> recipes;
        private RecipeData selectedRecipe;

        #region Unity Events

        private void Update() {
            // If we do not have any item selected, return
            if (selectedRecipe == null) {
                return;
            }

            if (Input.GetKeyDown(CraftKeyCode)) {
                OnItemActivated(GameManager.GetPlayer(), ingredientContainer, selectedRecipe);
            }
        }

        #endregion

        #region Event Callbacks

        private void OnRecipeSelected(RecipeData recipe) {
            selectedRecipe = recipe;
            RefreshSelectedRecipeFields();
        }

        public void OnItemActivated(IContainer target, IContainer source, RecipeData recipeData) {
            string displayName = Crafting.GetDisplayName(recipeData);

            if (!Crafting.HasIngredients(recipeData, source.Inventory)) {
                UiManager.Notify(string.Format(EngineSettings.SettingMsgNotEnoughIngredients, displayName));
            } else if (Crafting.Craft(recipeData, target.Inventory, source.Inventory)) {
                UiManager.Notify(string.Format(EngineSettings.SettingMsgItemCrafted, displayName));
            }
        }

        #endregion

        #region UI Functions

        private void RefreshRecipeList() {
            List<string> processedRecipes = new List<string>();

            foreach (string [] categoryPair in EngineSettings.CraftingCategories) {
                List<RecipeData> recipesInCat = null;
                if (!categoryPair[0].IsNullOrWhiteSpace()) {
                    recipesInCat = recipes.Where(x => GameTag.HasTag(x, categoryPair[0]) && !processedRecipes.Contains(x.Id)).ToList();
                } else {
                    recipesInCat = recipes.Where(x => x.GameTags.Length == 0 && !processedRecipes.Contains(x.Id)).ToList();
                }

                if (recipesInCat.Count > 0) {
                    AddCategoryHeader(recipeContainer, categoryPair[1]);

                    foreach (RecipeData recipe in recipesInCat) {
                        processedRecipes.Add(recipe.Id);

                        AddButton(recipeContainer, recipe, delegate {
                            OnRecipeSelected(recipe);
                        });
                    }
                }
            }
        }

        private void AddCategoryHeader(Transform container, string categoryName) {
            GameObject spawnedObject = Instantiate(categoryHeaderPrefab, container, false);
            spawnedObject.GetComponentInChildren<Text>().text = categoryName.ToUpper();
        }

        private void AddButton(Transform container, RecipeData recipeData, UnityAction callback) {
            GameObject spawnedObject = Instantiate(recipeButtonPrefab, container, false);
            spawnedObject.GetComponent<Button>().onClick.AddListener(callback);
            spawnedObject.GetComponent<RecipeListButton>().SetRecipe(recipeData);
        }

        private void RefreshSelectedRecipeFields() {
            if (selectedRecipe != null) {
                selectedRecipeNameLabel.text = Crafting.GetDisplayName(selectedRecipe);
                selectedRecipeDescrLabel.text = string.Empty;

                foreach (RecipeData.RecipeIngredient ingredient in selectedRecipe.Ingredients) {
                    selectedRecipeDescrLabel.text += $"{ingredient.itemData.DisplayName} ({ingredient.quantity}){Environment.NewLine}";
                }
            } else {
                selectedRecipeNameLabel.text = EngineSettings.SettingMsgUnknownRecipe;
                selectedRecipeDescrLabel.text = string.Empty;
            }
        }

        #endregion

        public void View(IContainer container) {
            if (container == null) {
                Debug.Log($"CraftingView - Ingredient container is NULL");
                return;
            }

            ingredientContainer = container;
            recipes = GameManager.DataManager.GetRecipes();
            selectedRecipe = null;

            RefreshRecipeList();
            RefreshSelectedRecipeFields();
            Show();
        }
    }
}