using JusticeFramework.Data;

namespace JusticeFramework.Logic {
    public class Codex {
        /// <summary>
        /// The amount of characters that should be displayed at once in-game.
        /// </summary>
        private const int CharactersPerPage = 250;

        /// <summary>
        /// Gets the current page number based on the index of the given character.
        /// </summary>
        /// <param name="data">Data for the codex.</param>
        /// <param name="charIndex">The index of the character</param>
        /// <returns>Returns the page number of the given character.</returns>
        public static int GetPageNumber(CodexData data, int charIndex) {
            return charIndex / CharactersPerPage;
        }

        /// <summary>
        /// Gets the substring of text for the given page number.
        /// </summary>
        /// <param name="data">Data for the codex.</param>
        /// <param name="pageNumber">The page number for the text</param>
        /// <returns>Returns the text for the given page number.</returns>
        public static string GetPageText(CodexData data, int pageNumber) {
            return data.Text.Substring(pageNumber * CharactersPerPage, CharactersPerPage);
        }
    }
}
