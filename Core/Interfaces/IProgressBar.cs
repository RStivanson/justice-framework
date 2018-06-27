namespace JusticeFramework.Core.Interfaces {
	public interface IProgressBar {
		void SetValue(float percentValue);
		void SetValue(float currentValue, float maxValue);
	}
}