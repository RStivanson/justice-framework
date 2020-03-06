using JusticeFramework.Core.Extensions;
using JusticeFramework.Interfaces;
using System.Linq;

namespace JusticeFramework.Logic {
    /// <summary>
    /// Handles the logic relating to game tags.
    /// </summary>
    public class GameTag {
        /// <summary>
        /// Determines if this object has the specified tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool HasTag(ITaggedData data, string tag) {
            return data?.GameTags.Any(x => x.Id.EqualsOrdinal(tag)) ?? false;
        }
    }
}
