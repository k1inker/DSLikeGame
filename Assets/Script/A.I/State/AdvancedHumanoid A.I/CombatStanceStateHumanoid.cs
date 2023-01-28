using UnityEngine;

namespace DS
{
    public class CombatStanceStateHumanoid : State
    {
        public AttackStateHumanoid attackState;
        public PursueTargetStateHumanoid pusueTargetState;

        [SerializeField] private ItemBasedAttackAction[] enemyAttacks;

        protected bool _randomDestinationSet = false;
        protected float _horizontalMovementValue = 0;
        protected float _verticalMovementValue = 0;

        [Header("State Flags")]
        private bool willPerformBlock = false;
        private bool willPerformDodge = false;
        private bool willPerformParry = false;
        public override State Tick(EnemyManager enemy)
        {
            if(enemy.combatStyle == AICombatStyle.swordAndShield)
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
            enemy.animator.SetFloat("Vertical", _verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", _horizontalMovementValue, 0.2f, Time.deltaTime);

            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);

                return this;
            }
            // if the A.I has gotten to far from it`s target?, return to pursue state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
                return pusueTargetState;

            //randomiez walking pattern of our A.I so they circle the player 
            if (!_randomDestinationSet)
            {
                _randomDestinationSet = true;
                DecideCirclingAction(enemy.enemyAnimatorManager);
            }

            RollDefenceAction(enemy);

            HandleRotateTowardsTarget(enemy);

            if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                _randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }
            return this;
        }

        private void RollDefenceAction(EnemyManager enemy)
        {
            if (enemy.allowAIToPreformBlock)
            {
                willPerformBlock = RollForActionChance(enemy.blockLikeHood);
            }

            if (enemy.allowAIToPreformDodge)
            {
                willPerformDodge = RollForActionChance(enemy.dodgeLikeHood);
            }

            if (enemy.allowAIToPreformParry)
            {
                willPerformParry = RollForActionChance(enemy.parryLikeHood);
            }

            if (willPerformBlock)
            {

            }
            if (willPerformDodge)
            {

            }
            if (willPerformParry)
            {

            }
        }

        private bool RollForActionChance(int likeHood)
        {
            int actionChance = Random.Range(0, 100);
            if(actionChance <= likeHood)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ResetStateFlags()
        {
            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
        }
        protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
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
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
        protected virtual void GetNewAttack(EnemyManager enemy)
        {
            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];
                if (InRange(enemyAttackAction, enemy.viewableAngle, enemy.distanceFromTarget))
                    maxScore += enemyAttackAction.attackScore;
            }
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];
                if (InRange(enemyAttackAction, enemy.viewableAngle, enemy.distanceFromTarget))
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
        protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            WalkAroundTarget(enemyAnimatorManager);
        }
        protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            _verticalMovementValue = 0.5f;

            _horizontalMovementValue = Random.Range(-1, 1);

            if (_horizontalMovementValue <= 1 && _horizontalMovementValue >= 0)
            {
                _horizontalMovementValue = 0.5f;
            }
            else if (_horizontalMovementValue >= -1 && _horizontalMovementValue < 0)
            {
                _horizontalMovementValue = -0.5f;
            }
        }
        protected bool InRange(ItemBasedAttackAction enemyAttackAction, float viewableAngle, float distanceFromTarget)
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