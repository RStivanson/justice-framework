using JusticeFramework.Interfaces;
using UnityEngine;

namespace JusticeFramework.Data {
    /// <summary>
    /// Data class for storing all data relating to Actor's (NPCs, Player, etc.)
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Actor Data")]
    public class ActorData : ScriptableDataObject, ISceneDataObject, IDisplayable {
        /// <summary>
        /// The name to be displayed in-game.
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
        /// The base maximum health of this actor before any modifications.
        /// </summary>
        [SerializeField]
        private int baseMaxHealth;

        /// <summary>
        /// Flag indicating if this actor can die.
        /// </summary>
        [SerializeField]
        private bool isInvincible;

        /// <summary>
        /// The prefab that represents this actor in the world.
        /// </summary>
        [SerializeField]
        private GameObject scenePrefab;

        /// <summary>
        /// A list of items to be given to this actor when spawned for the first time.
        /// </summary>
        [SerializeField]
        private ItemStackData[] defaultInventory;

        /// <summary>
        /// Data determine how this actors AI should function.
        /// </summary>
        [SerializeField]
        private AiData aiData;

        /// <summary>
        /// Gets the in-game displayed name.
        /// </summary>
        public string DisplayName {
            get { return displayName; }
        }

        /// <summary>
        /// Gets this actor's base max health.
        /// </summary>
        public int BaseMaxHealth {
            get { return baseMaxHealth; }
        }

        /// <summary>
        /// Gets if this actor can die.
        /// </summary>
        public bool IsInvincible {
            get { return isInvincible; }
        }

        /// <summary>
        /// Gets the prefab that represents this actor in the scene.
        /// </summary>
        public GameObject ScenePrefab {
            get { return scenePrefab; }
        }

        /// <summary>
        /// Gets the default items to be given to this actor
        /// </summary>
        public ItemStackData[] DefaultInventory {
            get { return defaultInventory; }
        }

        /// <summary>
        /// Gets the associated AI data for this actor.
        /// </summary>
        public AiData AiData {
            get { return aiData; }
        }
    }
}
