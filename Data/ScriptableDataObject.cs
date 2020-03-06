using JusticeFramework.Core;
using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    public abstract class ScriptableDataObject : ScriptableObject, IDataObject, ITaggedData {
        /// <summary>
        /// The game tags associated with this object.
        /// </summary>
        [SerializeField]
        private GameTagData[] gameTags;

        /// <summary>
        /// Gets this object's ID.
        /// </summary>
        public string Id {
            get { return name; }
            internal set { name = value; }
        }

        /// <summary>
        /// Gets the associated game tags.
        /// </summary>
        public GameTagData[] GameTags {
            get { return gameTags; }
            internal set { gameTags = value; }
        }
    }
}
