namespace JusticeFramework.Data.Events {
	public class ActivateEventArgs {
		public object Activator { get; set; }
		public object ActivatedBy { get; set; }
		
		public ActivateEventArgs(object activator, object activatedBy) {
			Activator = activator;
			ActivatedBy = activatedBy;
		}
	}
}