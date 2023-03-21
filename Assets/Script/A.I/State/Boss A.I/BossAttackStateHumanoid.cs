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
            ResetStateFlags();
            return rotateTowardsTargetState;
        }
        protected override void AttackTarget(EnemyManager enemy)
        {
            enemy.isUsingRightHand = currentAttackAction.isRightHandedAction;
            enemy.isUsingLeftHand = !currentAttackAction.isRightHandedAction;
            enemy.enemyAnimatorManager.PlayTargetAnimationWithRootMotion(currentAttackAction.actionAnimation, true);
            enemy.currentRecoveryTime = currentAttackAction.recoveryTime;
            currentAttackAction = null;
        }
    }
}