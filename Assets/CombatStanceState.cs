using UnityEngine;

namespace DS
{
    public class CombatStanceState : State
    {
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
            return this;
        }
    }
}