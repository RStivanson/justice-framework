using UnityEngine;

namespace JusticeFramework.Data {
    /// <inheritdoc />
    /// <summary>
    /// Data class for all codexes such as books, letters, etc.
    /// </summary>
    [CreateAssetMenu(menuName = "Justice Framework/Codex Data")]
    public class CodexData : ItemData {
        /// <summary>
        /// The text that is displayed in-game.
        /// </summary>
        [SerializeField]
        [TextArea(10, 30)]
        private string text = string.Empty;

        /// <summary>
        /// Gets the whole text that is displayed in-game. Use <see cref="GetPageText"/> for page specific text.
        /// </summary>
        public string Text {
            get { return text; }
        }
    }
}