namespace JusticeFramework.Core.Interfaces {
	/// <summary>
	/// Interface that defines attributes needed for entities
	/// </summary>
	public interface IEntity {
		/// <summary>
		/// Identifier to distinguish this entity
		/// </summary>
		string Id { get; }
	}
}