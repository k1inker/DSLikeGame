using UnityEngine;

namespace DS
{
    public class IdleStateHumanoid : State
    {
        private PursueTargetStateHumanoid _pursueTargetState;
        [SerializeField] private LayerMask _detectionLayer;
        private void Awake()
        {
            _pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
        }
        public override State Tick(EnemyManager enemy)
        {
            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.detectionRadius, _detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();
                if (character != null)
                {
                    if (character.characterStatsManager.teamIDNumber != enemy.enemyStatsManager.teamIDNumber)
                    {
                        Vector3 targetDirection = character.transform.position - enemy.transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                        if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                        {
                            enemy.currentTarget = character;
                        }
                    }
                }
            }
            #endregion

            #region Handle Switching Next State
            if (enemy.currentTarget != null)
            {
                return _pursueTargetState;
            }
            else
                return this;
            #endregion
        }
    }
}