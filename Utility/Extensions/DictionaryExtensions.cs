using System.Collections.Generic;

namespace JusticeFramework.Utility.Extensions {
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
    }
}
