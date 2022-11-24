using UnityEngine;

namespace DS
{
    public class AttackState : State
    {
        [SerializeField] private CombatStanceState combatStanceState;

        [SerializeField] private EnemyAttackAction[] enemyAttacks;
        [SerializeField] private EnemyAttackAction currentAttack;
        /// <summary>
        /// Select attack in attacks score
        /// if attack is not able on distance or angle, select A new attack
        /// if attack is viable, stop movement and attack target
        /// set attack recovery timer 
        /// </summary>
        /// <returns>combat stance state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            if (enemyManager.isPerformingAction)
                return combatStanceState;

            if (currentAttack != null)
            {
                if (distanceFromTarget < currentAttack.minimumDistanceToAttack)
                    return this;
                else if (distanceFromTarget < currentAttack.maximumDistanceToAttack)
                { 
                    if(viewableAngle <= currentAttack.maximumAttackAngle &&
                        viewableAngle >= currentAttack.minimumAttackAngle)
                    {
                        if(enemyManager.currentRecoveryTime <= 0 && !enemyManager.isPerformingAction)
                        {
                            enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                            currentAttack = null;
                            return combatStanceState;
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }
            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];
                if (InRange(enemyAttackAction, viewableAngle, distanceFromTarget))
                    maxScore += enemyAttackAction.attackScore;
            }
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];
                if (InRange(enemyAttackAction, viewableAngle, distanceFromTarget))
                {
                    if (currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        currentAttack = enemyAttackAction;
                    }
                }
            }
        }
        private bool InRange(EnemyAttackAction enemyAttackAction, float viewableAngle, float distanceFromTarget)
        {
            if (distanceFromTarget <= enemyAttackAction.maximumDistanceToAttack
                && distanceFromTarget >= enemyAttackAction.minimumDistanceToAttack)
                if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                    && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    return true;
            return false;
        }
    }
}