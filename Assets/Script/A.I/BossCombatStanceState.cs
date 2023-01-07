using UnityEngine;

namespace DS
{
    public class BossCombatStanceState : CombatStanceState
    {
        [Header("Second Phase Attacks")]
        [SerializeField] private EnemyAttackAction[] secondPhaseAttacks;
        public bool hasPhaseShifted;
        protected override void GetNewAttack(EnemyManager enemyManager)
        {
            if (hasPhaseShifted)
            {
                Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;

                float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                int maxScore = 0;
                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];
                    if (InRange(enemyAttackAction, viewableAngle, distanceFromTarget))
                        maxScore += enemyAttackAction.attackScore;
                }
                int randomValue = Random.Range(0, maxScore);
                int temporaryScore = 0;

                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    EnemyAttackAction enemyAttackAction = secondPhaseAttacks[i];
                    if (InRange(enemyAttackAction, viewableAngle, distanceFromTarget))
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
            else
            {
                base.GetNewAttack(enemyManager);
            }
        }
    }
}