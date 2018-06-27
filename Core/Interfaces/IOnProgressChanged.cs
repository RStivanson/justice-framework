namespace JusticeFramework.Core.Interfaces {
	public delegate void OnProgressChanged(bool done, float progress, string status = "");

	public interface IOnProgressChanged {
		event OnProgressChanged onProgressChanged;
	}
}