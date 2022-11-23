using UnityEngine;
namespace DS
{
    public class IdleState : State
    {
        [SerializeField] private PursueTargetState pursueTargetState;
        [SerializeField] private LayerMask _detectionLayer;
        /// <summary>
        /// Look for target
        /// if target is found switch to the Pursue state 
        /// </summary>
        /// <returns>if target not found return this state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, _detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].GetComponent<CharacterStats>();
                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Handle Switching Next State
            if (enemyManager.currentTarget != null)
                return pursueTargetState;
            else
                return this;
            #endregion
        }
    }
}