using System;

namespace JusticeFramework.Utility.Extensions {
	/// <summary>
	/// A collection of extension methods that extend the functionality of objects
	/// </summary>
	public static class ObjectExtensions {
		/// <summary>
		/// Determines if the object is of the given type
		/// </summary>
		/// <param name="left">The object whose type should be checked</param>
		/// <param name="type">The type that should be compared against</param>
		/// <returns>Returns true if the object is of the given type, false otherwise</returns>
		private static bool IsType(this object left, Type type) {
			return left.GetType() == type;
		}
		
		/// <summary>
		/// Determines if the object is of the given type, or sub-type
		/// </summary>
		/// <param name="left">The object whose type should be checked</param>
		/// <param name="type">The type that should be compared against</param>
		/// <returns>Returns true if the object is of the given type or sub-type, false otherwise</returns>
		public static bool IsTypeOrSubType(this object left, Type type) {
			return left.GetType() == type || left.GetType().IsSubclassOf(type);
		}
		
		/// <summary>
		/// Determines if the object is not of the given type
		/// </summary>
		/// <param name="left">The object whose type should be checked</param>
		/// <param name="type">The type that should be compared against</param>
		/// <returns>Returns true if the object is not of the given type, false otherwise</returns>
		public static bool NotType(this object left, Type type) {
			return !left.IsType(type);
		}
		
		/// <summary>
		/// Determines if the object is not of the given type, or sub-type
		/// </summary>
		/// <param name="left">The object whose type should be checked</param>
		/// <param name="type">The type that should be compared against</param>
		/// <returns>Returns true if the object is not of the given type or sub-type, false otherwise</returns>
		public static bool NotTypeOrSubType(this object left, Type type) {
			return !left.IsTypeOrSubType(type);
		}

        /// <summary>
        /// Determines if the object is of the given type
        /// </summary>
        /// <param name="left">The object whose type should be checked</param>
        /// <typeparam name="T">The type that should be compared against</typeparam>
        /// <returns>Returns true if the object is of the given type, false otherwise</returns>
        private static bool IsType<T>(this object left) {
			return left.IsType(typeof(T));
		}
		
		/// <summary>
		/// Determines if the object is of the given type, or sub-type
		/// </summary>
		/// <param name="left">The object whose type should be checked</param>
		/// <typeparam name="T">The type that should be compared against</typeparam>
		/// <returns>Returns true if the object is of the given type or sub-type, false otherwise</returns>
		public static bool IsTypeOrSubType<T>(this object left) {
			return left.IsTypeOrSubType(typeof(T));
		}
		
		/// <summary>
		/// Determines if the object is not of the given type
		/// </summary>
		/// <param name="left">The object whose type should be checked</param>
		/// <typeparam name="T">The type that should be compared against</typeparam>
		/// <returns>Returns true if the object is not of the given type, false otherwise</returns>
		public static bool NotType<T>(this object left) {
			return left.NotType(typeof(T));
		}
		
		/// <summary>
		/// Determines if the object is not of the given type, or sub-type
		/// </summary>
		/// <param name="left">The object whose type should be checked</param>
		/// <typeparam name="T">The type that should be compared against</typeparam>
		/// <returns>Returns true if the object is not of the given type or sub-type, false otherwise</returns>
		public static bool NotTypeOrSubType<T>(this object left) {
			return left.NotTypeOrSubType(typeof(T));
		}
	}
}