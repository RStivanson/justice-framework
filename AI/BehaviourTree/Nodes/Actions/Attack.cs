using JusticeFramework.Core;
using JusticeFramework.Core.AI.BehaviourTree;
using JusticeFramework.Core.AI.BehaviourTree.Nodes;

namespace JusticeFramework.AI.BehaviourTree.Nodes.Actions {
	public class Attack : Leaf {
		protected override ENodeStatus OnTick(TickState tick) {
			AiController controller = tick.blackboard.Get<AiController>("controller");
            EAttackStatus attackStatus = EAttackStatus.Empty;

            if (controller.Actor.IsAttacking) {
                attackStatus = controller.Actor.UpdateAttack();
            } else {
                controller.Actor.BeginAttack();
            }

			return (attackStatus == EAttackStatus.Building) ? ENodeStatus.Running : ENodeStatus.Success;
		}

        protected override void Cancel(TickState tick) {
            AiController controller = tick.blackboard.Get<AiController>("controller");

            if (controller.Actor.IsAttacking) {
                controller.Actor.EndAttack();
            }
        }
    }
}