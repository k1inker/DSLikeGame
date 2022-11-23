using UnityEngine;

namespace DS
{
    public class AttackState : State
    {
        /// <summary>
        /// Select attack in attacks score
        /// if attack is not able on distance or angle, select A new attack
        /// if attack is viable, stop movement and attack target
        /// set attack recovery timer 
        /// </summary>
        /// <returns>combat stance state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }
}