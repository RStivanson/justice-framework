using System;

namespace JusticeFramework.Utility.Extensions {
	public static  class ObjectExtensions {
		public static bool IsType(this object left, Type type) {
			var o = left.GetType();
			
			return left.GetType() == type;
		}
		
		public static bool IsTypeOrSubType(this object left, Type type) {
			return left.GetType() == type || left.GetType().IsSubclassOf(type);
		}
		
		public static bool NotType(this object left, Type type) {
			return !left.IsType(type);
		}
		
		public static bool NotTypeOrSubType(this object left, Type type) {
			return !left.IsTypeOrSubType(type);
		}
		
		public static bool IsType<T>(this object left) {
			return left.IsType(typeof(T));
		}
		
		public static bool IsTypeOrSubType<T>(this object left) {
			return left.IsTypeOrSubType(typeof(T));
		}
		
		public static bool NotType<T>(this object left) {
			return left.NotType(typeof(T));
		}
		
		public static bool NotTypeOrSubType<T>(this object left) {
			return left.NotTypeOrSubType(typeof(T));
		}
	}
}