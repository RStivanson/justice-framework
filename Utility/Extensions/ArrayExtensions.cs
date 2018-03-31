using System.Collections.Generic;

namespace JusticeFramework.Utility.Extensions {
	public static class ArrayExtensions {
		public static T[] Intersect<T>(this T[] array, T[] other) where T : class {
			List<T> overlap = new List<T>();
			int first, second;

			for (first = 0; first < array.Length; ++first) {
				for (second = 0; second < other.Length; ++second) {
					if (array[first] == other[second]) {
						overlap.Add(array[first]);
					}
				}
			}

			return overlap.ToArray();
		}
	}
}
