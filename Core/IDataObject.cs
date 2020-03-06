namespace JusticeFramework.Core {
    /// <summary>
    /// Base interface for identifiable objects that describe data.
    /// </summary>
    public interface IDataObject {
        /// <summary>
        /// The ID of this object.
        /// </summary>
        string Id { get; }
    }
}
