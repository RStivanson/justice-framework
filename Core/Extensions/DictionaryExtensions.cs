using System.Collections.Generic;

namespace JusticeFramework.Core.Extensions {
    /// <summary>
    /// A collection of extension methods that extend the functionality of dictionaries
    /// </summary>
    public static class DictionaryExtensions {
        /// <summary>
        /// Adds the given value to a list stored in the collection
        /// </summary>
        /// <typeparam name="T">The type used as the dictionaries key</typeparam>
        /// <typeparam name="S">The type used as the lists type</typeparam>
        /// <param name="collection">The dictionary where the value should be inserted</param>
        /// <param name="key">The key where the value should be inserted at</param>
        /// <param name="value">The value that should be stored in the dictionary</param>
        public static void AddToList<T, S>(this Dictionary<T, List<S>> collection, T key, S value) {
            // Attempt to get the list from the dicationary
            List<S> valueList;
            collection.TryGetValue(key, out valueList);

            // If it doesnt exist, create a new one and add it to the dicationary
            if (valueList == null) {
                valueList = new List<S> { value };
                collection.Add(key, valueList);
            } else {
                // Else add the value to the list
                valueList.Add(value);
            }
        }

        /// <summary>
        /// Trys to get the specified value from the dictionary or returns the default value if it fails.
        /// </summary>
        /// <typeparam name="T">The type of key in the dictionary.</typeparam>
        /// <typeparam name="S">The type of value in the dictionary.</typeparam>
        /// <param name="collection">The dictionary where the value is held.</param>
        /// <param name="key">The key identifying the value.</param>
        /// <param name="defaultValue">The value to fall back to. This defaults to the default value of the value type.</param>
        /// <returns>Returns the value in the dictionary assigned to the given key, else the default value if it fails.</returns>
        public static S GetOrDefault<T, S>(this Dictionary<T, S> collection, T key, S defaultValue = default) {
            if (collection.TryGetValue(key, out S value)) {
                return value;
            }

            return defaultValue;
        }
    }
}
