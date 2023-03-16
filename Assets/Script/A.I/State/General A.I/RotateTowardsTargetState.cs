using UnityEngine;

namespace DS
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);

            if (enemy.isInteracting)
                return this;

            if(enemy.viewableAngle >= 100 && enemy.viewableAngle <= 180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind Right", true);
                return combatStanceState;            
            }
            else if (enemy.viewableAngle <= -101 && enemy.viewableAngle >= -180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind Left", true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle <= -45 && enemy.viewableAngle >= -100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle >= 45 && enemy.viewableAngle <= 100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;
            }
            return combatStanceState;
             
        }
    }
}