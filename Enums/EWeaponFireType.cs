namespace JusticeFramework {
    /// <summary>
    /// The type of motion a weapon follows when firing
    /// </summary>
    public enum EWeaponFireType {
        /// <summary>
        /// The weapon uses a hitbox to determine the targets hit
        /// </summary>
        Hitbox,

        /// <summary>
        /// The weapon uses a raycast to determine the target hit
        /// </summary>
        Linear,

        /// <summary>
        /// The weapon spawns a projectile that determines the target hit
        /// </summary>
        Projectile
    }
}
