using System;
using UnityEngine;

namespace JusticeFramework.Core.Models.Crafting {
    /// <summary>
    /// Data class that stores the required items for a recipe
    /// </summary>
    [Serializable]
    public class RecipeItem {
        /// <summary>
        /// Reference to the object required for crafting
        /// </summary>
        [SerializeField]
        private WorldObjectModel ingredient;

        /// <summary>
        /// The amount of the objects required
        /// </summary>
        [SerializeField]
        private int quantity;

        /// <summary>
        /// Gets the ingredient required
        /// </summary>
        public WorldObjectModel Ingredient {
            get { return ingredient; }
        }

        /// <summary>
        /// Gets the quantity of the ingredient needed
        /// </summary>
        public int Quantity {
            get { return quantity; }
        }
    }
}
