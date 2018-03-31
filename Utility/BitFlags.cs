namespace JusticeFramework.Utility {
	public static class BitFlags {
		public static bool And(uint bitFlag, int bit) {
			return (bitFlag & bit) == bit;
		}
	}
}
