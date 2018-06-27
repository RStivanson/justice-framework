namespace JusticeFramework.Core.Interfaces {
    /// <inheritdoc />
    /// <summary>
    /// Interface that defines attributes needed for ammo
    /// </summary>
    public interface IAmmo : IItem {
        /// <summary>
        /// The type of ammo this falls under
        /// </summary>
        EAmmoType AmmoType { get; }

        /// <summary>
        /// The amount of damage this ammo applies
        /// </summary>
        float Damage { get; }
    }
}