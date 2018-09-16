using JusticeFramework.Core.Models;
using System.Collections.Generic;

namespace JusticeFramework.Core.Interfaces {
    public interface IHasStatusEffects {
        /// <summary>
        /// The status effects that will be applied to the target
        /// </summary>
        List<StatusEffectModel> StatusEffects { get; }
    }
}
