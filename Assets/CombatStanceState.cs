using UnityEngine;

namespace DS
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pusueTargetState;
        /// <summary>
        /// check attack range
        /// if in attack range return attack state
        /// </summary>
        /// <returns>
        /// if in attack range return attack state, 
        /// if cool down after attack return this state
        /// if target out of range return purseTarget state
        /// </returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.distanceFromTarget <= enemyManager.maximumAttackRange)
                return attackState;
            else if(enemyManager.distanceFromTarget > enemyManager.maximumAttackRange)
                return pusueTargetState;
            else
                return this;
        }
    }
}