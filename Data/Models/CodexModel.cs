using System;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Model class for all codexes such as books, letters, etc.
	/// </summary>
	[Serializable]
	public class CodexModel : ItemModel {
		/// <summary>
		/// The text that can be read by the player
		/// </summary>
		public string text = string.Empty;
	}
}