namespace JusticeFramework.Interfaces {
    public delegate void OnStateChanged<T>(T changed);

    public interface IOnStateChanged<T> {
        event OnStateChanged<T> onStateChanged;
	}
}