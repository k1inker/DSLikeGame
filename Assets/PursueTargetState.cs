using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{
    public class PursueTargetState : State
    {
        /// <summary>
        /// Chase the target
        /// if in attack range switch to Combat stance state
        /// </summary>
        /// <returns>if target out of range return this state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }
}