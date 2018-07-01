namespace JusticeFramework.Core.Interfaces {
	/// <inheritdoc />
	/// <summary>
	/// Interface that defines attributes needed for codexes and books
	/// </summary>
	public interface ICodex : IItem {
		/// <summary>
		/// The contents of the codex
		/// </summary>
		string Text { get; }
	}
}