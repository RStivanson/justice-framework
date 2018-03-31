using System;
using System.Collections.Generic;
using UnityEngine;

namespace JusticeFramework.Utility {
	[Serializable]
	public class Vector2i {
		[SerializeField]
		private int x;

		[SerializeField]
		private int y;

		public int X {
			get {
				return x;
			}
		}

		public int Y {
			get {
				return y;
			}
		}

		public float magnitude {
			get {
				return Mathf.Sqrt(sqrMagnitude);
			}
		}

		public float sqrMagnitude {
			get {
				return (x * x) + (y * y);
			}
		}

		public static Vector2i zero {
			get {
				return new Vector2i(0, 0);
			}
		}

		public static Vector2i one {
			get {
				return new Vector2i(1, 1);
			}
		}

		public static Vector2i min {
			get {
				return new Vector2i(int.MinValue, int.MinValue);
			}
		}

		public static Vector2i max {
			get {
				return new Vector2i(int.MaxValue, int.MaxValue);
			}
		}

		public Vector2i() {
			x = 0;
			y = 0;
		}

		public Vector2i(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public Vector2i(Vector2i vector) {
			x = vector.x;
			y = vector.y;
		}

		public static Vector2i[] NeighborsCircle(Vector2i center, int radius = 3, bool thin = false) {
			List<Vector2i> coords = new List<Vector2i>();
			
			int xOffset = 0, yOffset = 0;

			int centerX = center.x;
			int centerY = center.y;

			int yStart = center.y - radius, yEnd = center.y + radius;
			int xStart = center.x - radius, xEnd = center.x + radius;
			
			if (yStart < 0) {
				yOffset = -yStart;
				centerY += yOffset;
				yStart += yOffset;
				yEnd += yOffset;
			}

			if (xStart < 0) {
				xOffset = -xStart;
				centerX += xOffset;
				xStart += xOffset;
				xEnd += xOffset;
			}

			float distance;
			float radiusSqr = radius * radius;
	
			bool inCircle, onCircle;
			
			for (int x = xStart; x <= xEnd; ++x) {
				for (int y = yStart; y <= yEnd; ++y) {
					distance = ((x - centerX) * (x - centerX)) + ((y - centerY) * (y - centerY));
				
					inCircle = distance < radiusSqr;
					onCircle = distance == radiusSqr;
					
					if (onCircle && !thin) {
						coords.Add(new Vector2i(x - xOffset, y - yOffset));
					} else if (inCircle) {
						coords.Add(new Vector2i(x - xOffset, y - yOffset));
					}
			   }
			}

			return coords.ToArray();
		}
		
		public static Vector2i[] NeighborsSquare(Vector2i coords, int chunks = 3) {
			Vector2i[] neighbors = new Vector2i[chunks * chunks];

			// Get the max and min ending bounds
			// ex. chunks = 3 then ends are -1 and 1
			// so : floor(3 / 2) = floor (1.5) = 1
			int end = (int)Mathf.Floor(chunks / 2.0f);

			// The first one will always be the cell coordinates given to us
			neighbors[0] = coords;
			int count = 1;

			// current position minus and plus the ends to get the neighbors we need
			// <= because if it is just less than then we will miss the upper end
			for (int x = coords.x - end; x <= coords.x + end; ++x) {
				for (int y = coords.y - end; y <= coords.y + end; ++y) {
					// Skip the starting cell because it is already in index 0
					if (x == coords.x && y == coords.y)
						continue;

					neighbors[count] = new Vector2i(x, y);
					count++;
				}
			}

			return neighbors;
		}
		
		public static bool operator==(Vector2i left, Vector2i right) {
			return (left.x == right.x) && (left.y == right.y);
		}

		public static bool operator!=(Vector2i left, Vector2i right) {
			return (left.x != right.x) || (left.y != right.y);
		}

		public override bool Equals(object obj) {
			if (obj is Vector2i) {
				return (this == (obj as Vector2i));
			}

			return false;
		}

		public override int GetHashCode() {
			unchecked { // Overflow is fine, just wrap
				int hash = 17;
				hash = hash * 23 + x;
				hash = hash * 23 + y;

				return hash;
			}
		}

		public override string ToString() {
			return string.Format("({0}, {1})", x, y);
		}
	}
}