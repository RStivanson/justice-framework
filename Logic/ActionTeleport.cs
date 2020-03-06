using JusticeFramework.Components;
using JusticeFramework.Managers;
using UnityEngine;

namespace JusticeFramework.Logic {
    public class ActionTeleport : Action {
        private string targetScene;
        private Vector3 targetPosition;
        private Quaternion targetRotation;

        public ActionTeleport(WorldObject target, string targetScene, Vector3 targetPos, Quaternion targetRot) : base(target) {
            this.targetScene = targetScene;
            targetPosition = targetPos;
            targetRotation = targetRot;
        }

        protected override void OnExecute(WorldObject actor) {
            if (GameManager.IsPlayer(actor)) {
                GameManager.SendPlayerToScene(targetScene, targetPosition, targetRotation.eulerAngles);
            }
        }
    }
}
