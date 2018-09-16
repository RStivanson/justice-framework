using System;
using UnityEngine;

namespace JusticeFramework.Core.Collections {
    /// <summary>
    /// Collection class that holds information regarding a stack
    /// </summary>
    [Serializable]
    public class Stack {
        /// <summary>
        /// The current amount of items being held in this stack
        /// </summary>
        [SerializeField]
        private int currentQuantity;
        
        /// <summary>
        /// Gets the current stack capacity
        /// </summary>
        public int Quantity {
            get { return currentQuantity; }
        }

        /// <summary>
        /// Constructs a new stack object
        /// </summary>
        /// <param name="currentAmount">The current stack capacity</param>
        /// <param name="maxAmount">The max stack capacity</param>
        public Stack(int currentAmount = 0) {
            currentQuantity = currentAmount;
        }

        /// <summary>
        /// Constructs a new stack object copying values from another stack
        /// </summary>
        /// <param name="stack">The item stack to copy values from</param>
        public Stack(Stack stack) {
            currentQuantity = stack.currentQuantity;
        }

        /// <summary>
        /// Adds the given amount to the stack
        /// </summary>
        /// <param name="amount">The amount to add</param>
        public void Add(int amount) {
            // Add the amount to the stack
            currentQuantity += amount;
        }

        /// <summary>
        /// Removes the amount of items from the given stack
        /// </summary>
        /// <param name="amount">The amount of items to remove from the stack</param>
        /// <returns>Returns the capacity remaining</returns>
        public int Remove(int amount) {
            int amountRemoved = 0;
            int previous = currentQuantity;

            // Remove the quantity from the stack
            currentQuantity -= amount;

            // If we are under zero
            if (currentQuantity < 0) {
                // Calculate the actual amount removed
                amountRemoved = previous;

                // Reset the stack to 0
                currentQuantity = 0;
            } else {
                // We removed everything
                amountRemoved = amount;
            }

            return amount - amountRemoved;
        }
    }
}
