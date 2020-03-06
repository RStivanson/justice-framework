using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JusticeFramework {
    /// <summary>
    /// Types of ammo for ammo-based weapons
    /// </summary>
    [Flags]
    public enum EAmmoType {
        Any = 0,
        Arrow = 1,
        Bolt = 2,
        Bullet = 4
    }
}
