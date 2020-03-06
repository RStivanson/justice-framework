using JusticeFramework.Core.AI.BehaviourTree;

namespace JusticeFramework.AI.BehaviourNodes {
    public class Attack : Leaf {
		protected override ENodeStatus OnTick(BehaviourTree.Context tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
            EAttackStatus attackStatus = EAttackStatus.Empty;

            if (controller.Actor.IsAttacking) {
                attackStatus = controller.Actor.UpdateAttack();
            } else {
                controller.Actor.BeginAttack();
            }

			return (attackStatus == EAttackStatus.Building) ? ENodeStatus.Running : ENodeStatus.Success;
		}

        protected override void Cancel(BehaviourTree.Context tick) {
            AiController controller = tick.blackboard.Get<AiController>("controller");

            if (controller.Actor.IsAttacking) {
                controller.Actor.EndAttack();
            }
        }
    }
}