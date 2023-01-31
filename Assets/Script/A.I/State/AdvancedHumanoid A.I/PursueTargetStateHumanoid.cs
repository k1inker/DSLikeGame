using UnityEngine;
namespace DS
{
    public class PursueTargetStateHumanoid : State
    {
        private CombatStanceStateHumanoid _combatStanceState;
        private void Awake()
        {
            _combatStanceState = GetComponent<CombatStanceStateHumanoid>();   
        }
        public override State Tick(EnemyManager enemy)
        {
            if (enemy.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }
        private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
        {
            HandleRotateTowardsTarget(enemy);

            if (enemy.isInteracting)
                return this;

            if (enemy.isPerformingAction)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }


            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
                return _combatStanceState;
            else
                return this;
        }
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed);
            ////Rotate manualy
            //if (enemyManager.isPerformingAction)
            //{
            //    Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            //    direction.y = 0;
            //    direction.Normalize();

            //    if (direction == Vector3.zero)
            //    {
            //        direction = enemyManager.transform.forward;
            //    }

            //    Quaternion targetRotation = Quaternion.LookRotation(direction);
            //    enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            //}
            ////Rotate with navmesh
            //else
            //{
            //    Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
            //    Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

            //    enemyManager.navmeshAgent.enabled = true;
            //    enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            //    enemyManager.enemyRigidbody.velocity = targetVelocity;
            //    enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            //}
        }
    }
}