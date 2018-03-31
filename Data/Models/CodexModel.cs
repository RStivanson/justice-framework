using System;
using UnityEngine;

namespace JusticeFramework.Data.Models {
	/// <inheritdoc />
	/// <summary>
	/// Model class for all codexes such as books, letters, etc.
	/// </summary>
	[Serializable]
	public class CodexModel : ItemModel {
		public const int WORDS_PER_PAGE = 256;
		
		/// <summary>
		/// The text that can be read by the player
		/// </summary>
		public string text = string.Empty;
	}
}