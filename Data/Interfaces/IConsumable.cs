namespace JusticeFramework.Data.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for consumables
	/// </summary>
	public interface IConsumable : IItem {
		/// <summary>
		/// The amount of health to add when consumed
		/// </summary>
		int HealthModifier { get; }
	}
}