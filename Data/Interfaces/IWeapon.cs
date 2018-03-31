namespace JusticeFramework.Data.Interfaces {
	public interface IWeapon : IEquippable {
		EWeaponType WeaponType { get; }
		
		int Damage { get; }
	}
}