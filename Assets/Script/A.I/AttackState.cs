using UnityEngine;

namespace DS
{
    public class AttackState : State
    {
        [SerializeField] private CombatStanceState combatStanceState;
        [SerializeField] private PursueTargetState pursueTargetState;
        [SerializeField] private RotateTowardsTargetState rotateTowardsTargetState;
        
        public EnemyAttackAction currentAttack;
        public bool hasPerformedAttack = false;

        private bool _willDoCombo = false;
        /// <summary>
        /// Select attack in attacks score
        /// if attack is not able on distance or angle, select A new attack
        /// if attack is viable, stop movement and attack target
        /// set attack recovery timer 
        /// </summary>
        /// <returns>combat stance state</returns>
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            RotateTowardsTargetWhilstAttacking(enemyManager);

            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(_willDoCombo && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyAnimatorManager, enemyManager);
                
            }
            if(!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorManager,enemyManager);
                RollComboChance(enemyManager);
            }
            if(_willDoCombo && hasPerformedAttack)
            {
                return this;
            }

            return rotateTowardsTargetState;
        }
        private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            _willDoCombo = false;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }
        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }
        private void RollComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);
            if(enemyManager.AIPerfomCombos && comboChance <= enemyManager.comboLikeliHood)
            {
                if (currentAttack.comboNextAction != null)
                {
                    _willDoCombo = true;
                    currentAttack = currentAttack.comboNextAction;
                }
                else
                {
                    _willDoCombo = false;
                    currentAttack = null;
                }
            }
        }

        private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            //Rotate manualy
            if (enemyManager.canRotate && enemyManager.isInteracting)
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
    }
}