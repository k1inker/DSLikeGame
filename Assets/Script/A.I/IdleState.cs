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
        public override State Tick(EnemyManager enemy)
        {
            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.detectionRadius, _detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStatsManager characterStats = colliders[i].GetComponent<CharacterStatsManager>();
                
                if (characterStats != null)
                {
                    if (characterStats.teamIDNumber != enemy.enemyStatsManager.teamIDNumber)
                    {
                        Vector3 targetDirection = characterStats.transform.position - enemy.transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                        if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                        {
                            enemy.currentTarget = characterStats;
                        }
                    }
                }
            }
            #endregion

            #region Handle Switching Next State
            if (enemy.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
                return this;
            #endregion
        }
    }
}