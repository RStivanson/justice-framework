namespace JusticeFramework.Utility {
	[System.Serializable]
	public class Vector3i {
		[UnityEngine.SerializeField]
		private int x;

		[UnityEngine.SerializeField]
		private int y;
		
		[UnityEngine.SerializeField]
		private int z;

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

		public int Z {
			get {
				return z;
			}
		}

		public float magnitude {
			get {
				return UnityEngine.Mathf.Sqrt(sqrMagnitude);
			}
		}

		public float sqrMagnitude {
			get {
				return (x * x) + (y * y) + (z * z);
			}
		}

		public static Vector3i zero {
			get {
				return new Vector3i(0, 0, 0);
			}
		}

		public static Vector3i one {
			get {
				return new Vector3i(1, 1, 1);
			}
		}

		public static Vector3i min {
			get {
				return new Vector3i(int.MinValue, int.MinValue, int.MinValue);
			}
		}

		public static Vector3i max {
			get {
				return new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);
			}
		}
		
		public Vector3i() {
			x = 0;
			y = 0;
			z = 0;
		}

		public Vector3i(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3i(Vector3i vector) {
			x = vector.x;
			y = vector.y;
			z = vector.z;
		}
		
		public static bool operator==(Vector3i left, Vector3i right) {
			return (left.x == right.x) && (left.y == right.y) && (left.z == right.z);
		}

		public static bool operator!=(Vector3i left, Vector3i right) {
			return (left.x != right.x) || (left.y != right.y) || (left.z != right.z);
		}

		public override bool Equals(object obj) {
			if (obj is Vector3i) {
				return (this == (obj as Vector3i));
			}

			return false;
		}

		public override int GetHashCode() {
			unchecked { // Overflow is fine, just wrap
				int hash = 17;
				hash = hash * 23 + x;
				hash = hash * 23 + y;
				hash = hash * 23 + z;

				return hash;
			}
		}

		public override string ToString() {
			return string.Format("({0}, {1}, {2})", x, y, z);
		}
	}
}