using JusticeFramework.Components;

namespace JusticeFramework.Logic {
    public class ActionEmpty : Action {
        public ActionEmpty() : base(null, null, EAudioType.Ambient, 0) {
        }

        public override bool IsEmptyAction {
            get { return true; }
        }

        protected override void OnExecute(WorldObject actor) {
        }
    }
}
