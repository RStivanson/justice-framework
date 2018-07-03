using System;
using JusticeFramework.Core.Models.Crafting;
using UnityEngine;
using UnityEngine.UI;

namespace JusticeFramework.UI.Components {
	/// <inheritdoc />
	/// <summary>
	/// Basic model container script attached to each button in the crafting screen list
	/// </summary>
	[Serializable]
	public class RecipeListButton : MonoBehaviour {
		/// <summary>
		/// The main button script on this object
		/// </summary>
		[SerializeField]
		private Button buttonScript;
		
		/// <summary>
		/// The recipe name GUI text component
		/// </summary>
		[SerializeField]
		private Text recipeNameText;

		/// <summary>
		/// The recipe data this button relates to
		/// </summary>
		[SerializeField]
		private Recipe recipe;

		/// <summary>
		/// Gets the recipe attached to this object
		/// </summary>
		public Recipe Recipe {
			get { return recipe; }
		}
		
		/// <summary>
		/// Attaches the recipe to this object
		/// </summary>
		/// <param name="recipe">The recipe data</param>
		public void SetRecipe(Recipe recipe) {
			this.recipe = recipe;
			
			RefreshFields();
		}

		/// <summary>
		/// Refreshes the text components with the stored model's values
		/// </summary>
		private void RefreshFields() {
			if (recipe == null) {
				return;
			}

			recipeNameText.text = recipe.Result.Ingredient.displayName;
		}
	}
}
