namespace JusticeFramework.Data.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for armor
	/// </summary>
	public interface IArmor : IEquippable {
		/// <summary>
		/// The defense rating of this armor piece
		/// </summary>
		int ArmorRating { get; }
	}
}