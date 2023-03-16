
using UnityEngine;

namespace DS {
    public class BossAttackStateHumanoid : AttackStateHumanoid
    {
        public EnemyAttackAction currentAttackAction;
        public override State Tick(EnemyManager enemy)
        {
            RotateTowardsTargetWhilstAttacking(enemy);

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            AttackTarget(enemy);
            Debug.Log(1);
            ResetStateFlags();
            return rotateTowardsTargetState;
        }
        protected override void AttackTarget(EnemyManager enemy)
        {
            enemy.enemyAnimatorManager.PlayTargetAnimationWithRootMotion(currentAttackAction.actionAnimation, true);
            enemy.currentRecoveryTime = currentAttackAction.recoveryTime;
            currentAttackAction = null;
        }
    }
}