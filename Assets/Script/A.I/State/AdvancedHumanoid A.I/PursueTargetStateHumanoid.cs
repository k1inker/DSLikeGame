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
            if (enemy.combatStyle == AICombatStyle.swordAndShield || enemy.combatStyle == AICombatStyle.heavySword)
            {
                return ProcessCombatStyle(enemy);
            }
            else if(enemy.combatStyle == AICombatStyle.boss)
            {
                return ProcessCombatStyle(enemy, 0.3f);
            }
            else
            {
                return this;
            }
        }
        private State ProcessCombatStyle(EnemyManager enemy, float defaultSpeed = 1f)
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
                enemy.animator.SetFloat("Vertical", defaultSpeed, 0.1f, Time.deltaTime);
            }

            if (enemy.distanceFromTarget <= enemy.maximumAggroRadius)
                return _combatStanceState;
            else
                return this;
        }
        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
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
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}