using UnityEngine;

namespace DS
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pusueTargetState;

        [SerializeField] private EnemyAttackAction[] enemyAttacks;

        private bool _randomDestinationSet = false;
        private float _horizontalMovementValue = 0;
        private float _verticalMovementValue = 0;
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
            enemyAnimatorManager.anim.SetFloat("Vertical", _verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorManager.anim.SetFloat("Horizontal", _horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0);
                enemyAnimatorManager.anim.SetFloat("Horizontal", 0);

                return this;
            }

            if(distanceFromTarget > enemyManager.maximumAggroRadius)
                return pusueTargetState;

            if(!_randomDestinationSet)
            {
                _randomDestinationSet = true;
                DecideCirclingAction(enemyAnimatorManager);
            }

            HandleRotateTowardsTarget(enemyManager,distanceFromTarget);


            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                _randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }
            return this;
        }
        private void HandleRotateTowardsTarget(EnemyManager enemyManager, float distanceFromTarget)
        {
            //Rotate manualy
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = enemyManager.transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            //Rotate with navmesh
            else
            {
                //enemyManager.navmeshAgent.enabled = true;
                //enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                //float rotationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation, Quaternion.LookRotation(enemyManager.navmeshAgent.desiredVelocity.normalized));
                //if (distanceFromTarget > 5) enemyManager.navmeshAgent.angularSpeed = 500f;
                //else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) < 30) enemyManager.navmeshAgent.angularSpeed = 50f;
                //else if (distanceFromTarget < 5 && Mathf.Abs(rotationToApplyToDynamicEnemy) > 30) enemyManager.navmeshAgent.angularSpeed = 500f;

                //Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                //Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);


                //if (enemyManager.navmeshAgent.desiredVelocity.magnitude > 0)
                //{
                //    enemyManager.navmeshAgent.updateRotation = false;
                //    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                //    Quaternion.LookRotation(enemyManager.navmeshAgent.desiredVelocity.normalized), enemyManager.navmeshAgent.angularSpeed * Time.deltaTime);
                //}
                //else
                //{
                //    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navmeshAgent.angularSpeed * Time.deltaTime);
                //}
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
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

        private void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            WalkAroundTarget(enemyAnimatorManager);
        }
        private void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            _verticalMovementValue = 0.5f;

            _horizontalMovementValue = Random.Range(-1, 1);

            if(_horizontalMovementValue <= 1 && _horizontalMovementValue >= 0)
            {
                _horizontalMovementValue = 0.5f;
            }
            else if(_horizontalMovementValue >= -1 && _horizontalMovementValue < 0)
            {
                _horizontalMovementValue = -0.5f;
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