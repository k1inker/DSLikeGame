using UnityEngine;

namespace DS
{
    public class AttackStateHumanoid : State
    {
        private CombatStanceStateHumanoid _combatStanceState;
        private PursueTargetStateHumanoid _pursueTargetState;
        private RotateTowardsStateHumanoid _rotateTowardsTargetState;

        public ItemBasedAttackAction currentAttack;
        public bool hasPerformedAttack = false;

        [SerializeField]private bool _willDoCombo = false;
        private void Awake()
        {
            _pursueTargetState = GetComponent<PursueTargetStateHumanoid>();
            _rotateTowardsTargetState = GetComponent<RotateTowardsStateHumanoid>();
            _combatStanceState = GetComponent<CombatStanceStateHumanoid>();
        }
        /// <summary>
        /// Select attack in attacks score
        /// if attack is not able on distance or angle, select A new attack
        /// if attack is viable, stop movement and attack target
        /// set attack recovery timer 
        /// </summary>
        /// <returns>combat stance state</returns>
        public override State Tick(EnemyManager enemy)
        {
            RotateTowardsTargetWhilstAttacking(enemy);

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return _pursueTargetState;
            }

            if (_willDoCombo && enemy.canDoCombo)
            {
                AttackTargetWithCombo(enemy);
            }
            else if (_willDoCombo && !enemy.canDoCombo && !enemy.isAttacking)
            {
                ResetStateFlags();
                currentAttack = null;
                return _combatStanceState;
            }
            if (!hasPerformedAttack)
            {
                AttackTarget(enemy);
                RollComboChance(enemy);
            }
            if (_willDoCombo && hasPerformedAttack)
            {
                return this;
            }

            ResetStateFlags();
            return _rotateTowardsTargetState;
        }
        private void AttackTarget(EnemyManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }
        private void AttackTargetWithCombo(EnemyManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            _willDoCombo = false;
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }
        private void RollComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);
            if (enemyManager.AIPerfomCombos && comboChance <= enemyManager.comboLikeliHood)
            {
                if (currentAttack.actionCanCombo)
                {
                    _willDoCombo = true;
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
                //Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                //Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                //enemyManager.navmeshAgent.enabled = true;
                //enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                //enemyManager.enemyRigidbody.velocity = targetVelocity;
                //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed);
            }
        }
        private void ResetStateFlags()
        {
            _willDoCombo = false;
            hasPerformedAttack = false;
        }
    }
}