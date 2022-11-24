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
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if (enemyManager.isPerformingAction)
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);

            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
                return attackState;
            else if(distanceFromTarget > enemyManager.maximumAttackRange)
                return pusueTargetState;
            else
                return this;
        }
    }
}