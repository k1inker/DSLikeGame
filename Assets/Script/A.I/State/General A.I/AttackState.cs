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
        public override State Tick(EnemyManager enemy)
        {
            float distanceFromTarget = Vector3.Distance(enemy.currentTarget.transform.position, enemy.transform.position);

            RotateTowardsTargetWhilstAttacking(enemy);

            if(distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(_willDoCombo && enemy.canDoCombo)
            {
                AttackTargetWithCombo(enemy);
                
            }
            if(!hasPerformedAttack)
            {
                AttackTarget(enemy);
                RollComboChance(enemy);
            }
            if(_willDoCombo && hasPerformedAttack)
            {
                return this;
            }

            return rotateTowardsTargetState;
        }
        private void AttackTarget(EnemyManager enemy)
        {
            enemy.isUsingRightHand = currentAttack.isRightHandedAction;
            enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;
            enemy.enemyAnimatorManager.PlayTargetAnimationWithRootMotion(currentAttack.actionAnimation, true);
            enemy.enemyAnimatorManager.PlayWeaponTrailFX();
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }
        private void AttackTargetWithCombo(EnemyManager enemy)
        {
            enemy.isUsingRightHand = currentAttack.isRightHandedAction;
            enemy.isUsingLeftHand = !currentAttack.isRightHandedAction;

            _willDoCombo = false;
            enemy.enemyAnimatorManager.PlayTargetAnimationWithRootMotion(currentAttack.actionAnimation, true);
            enemy.enemyAnimatorManager.PlayWeaponTrailFX();
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
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