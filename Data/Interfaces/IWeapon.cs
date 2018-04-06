namespace JusticeFramework.Data.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for weapons
	/// </summary>
	public interface IWeapon : IEquippable {
		/// <summary>
		/// The type of weapon
		/// </summary>
		EWeaponType WeaponType { get; }
		
		/// <summary>
		/// The amount of damage dealt by this weapon
		/// </summary>
		int Damage { get; }
	}
}