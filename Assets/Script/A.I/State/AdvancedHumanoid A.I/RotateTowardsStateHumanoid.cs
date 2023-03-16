namespace DS
{
    public class RotateTowardsStateHumanoid : State
    {
        private CombatStanceStateHumanoid _combatStanceState;
        private void Awake()
        {
            _combatStanceState = GetComponent<CombatStanceStateHumanoid>();
        }
        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);

            if (enemy.isInteracting)
                return this;

            if (enemy.isBoss)
                _combatStanceState = _combatStanceState as BossCombatStanceStateHumanoid;

            if (enemy.viewableAngle >= 100 && enemy.viewableAngle <= 180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind Right", true);
                return _combatStanceState;
            }
            else if (enemy.viewableAngle <= -101 && enemy.viewableAngle >= -180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind Left", true);
                return _combatStanceState;
            }
            else if (enemy.viewableAngle <= -45 && enemy.viewableAngle >= -100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return _combatStanceState;
            }
            else if (enemy.viewableAngle >= 45 && enemy.viewableAngle <= 100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return _combatStanceState;
            }
            return _combatStanceState;

        }
    }
}