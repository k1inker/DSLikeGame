using UnityEngine;

namespace DS
{
    public class BossCombatStanceState : CombatStanceState
    {
        [Header("Second Phase Attacks")]
        [SerializeField] private EnemyAttackAction[] secondPhaseAttacks;
        public bool hasPhaseShifted;
        public override State Tick(EnemyManager enemy)
        {
            return base.Tick(enemy);
        }
        protected override void GetNewAttack(EnemyManager enemy)
        {
            if (hasPhaseShifted)
            {
                Vector3 targetDirection = enemy.currentTarget.transform.position - transform.position;

                float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);
                float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

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
                base.GetNewAttack(enemy);
            }
        }
    }
}