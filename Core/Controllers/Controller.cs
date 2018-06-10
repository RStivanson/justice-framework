namespace JusticeFramework.Core.Controllers {
	public class Controller : UnityEngine.MonoBehaviour {
		public void Freeze() {
			enabled = false;
		}

		public void Unfreeze() {
			enabled = true;
		}
	}
}
