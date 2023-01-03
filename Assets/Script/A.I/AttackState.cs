using UnityEngine;

namespace DS
{
    public class AttackState : State
    {
        [SerializeField] private CombatStanceState combatStanceState;

        [SerializeField] private EnemyAttackAction[] enemyAttacks;
        [SerializeField] private EnemyAttackAction currentAttack;

        private bool _willDoCombo = false;
        /// <summary>
        /// Select attack in attacks score
        /// if attack is not able on distance or angle, select A new attack
        /// if attack is viable, stop movement and attack target
        /// set attack recovery timer 
        /// </summary>
        /// <returns>combat stance state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isInteracting && !enemyManager.canDoCombo)
            {
                return this;
            }
            else if(enemyManager.isInteracting && enemyManager.canDoCombo)
            {
                if (_willDoCombo)
                {
                    _willDoCombo = false;
                    enemyAnimatorManager.PlayTargetAnimationWithRootMotion(currentAttack.actionAnimation, true);
                }
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            HandleRotateTowardsTarget(enemyManager, distanceFromTarget);

            if (enemyManager.isPerformingAction)
            {
                return combatStanceState;
            }
            

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
                            enemyAnimatorManager.PlayTargetAnimationWithRootMotion(currentAttack.actionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            RollComboChance(enemyManager);

                            if(currentAttack.canCombo && _willDoCombo)
                            {
                                currentAttack = currentAttack.comboNextAction;
                                return this;
                            }
                            else
                            {
                                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                                currentAttack = null;
                                return combatStanceState;
                            }
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

        private void RollComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);
            if(enemyManager.AIPerfomCombos && comboChance <= enemyManager.comboLikeliHood)
            {
                _willDoCombo = true;
            }
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