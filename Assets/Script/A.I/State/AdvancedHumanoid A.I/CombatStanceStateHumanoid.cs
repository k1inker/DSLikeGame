using UnityEngine;
using UnityEngine.UIElements;

namespace DS
{
    public class CombatStanceStateHumanoid : State
    {
        private AttackStateHumanoid _attackState;
        private PursueTargetStateHumanoid _pusueTargetState;

        [SerializeField] private ItemBasedAttackAction[] enemyAttacks;

        protected bool _randomDestinationSet = false;
        protected float _horizontalMovementValue = 0;
        protected float _verticalMovementValue = 0;

        [Header("State Flags")]
        private bool _willPerformBlock = false;
        private bool _willPerformDodge = false;
        private bool _willPerformParry = false;

        private bool _hasPreformedDodge = false;
        private bool _hasRandomDodgeDirection = false;
        private bool _hasPerformedParry = false;

        private Quaternion _targetDodgeDirection;
        private void Awake()
        {
            _attackState = GetComponent<AttackStateHumanoid>();
            _pusueTargetState = GetComponent<PursueTargetStateHumanoid>();
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
            enemy.animator.SetFloat("Vertical", _verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", _horizontalMovementValue, 0.2f, Time.deltaTime);

            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);

                return this;
            }
            if(enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return this;    
            }
            // if the A.I has gotten to far from it`s target?, return to pursue state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();  
                return _pusueTargetState;
            }

            //randomiez walking pattern of our A.I so they circle the player 
            if (!_randomDestinationSet)
            {
                _randomDestinationSet = true;
                DecideCirclingAction(enemy.enemyAnimatorManager);
            }

            RollDefenceAction(enemy);

            HandleRotateTowardsTarget(enemy);

            if (enemy.currentRecoveryTime <= 0 && _attackState.currentAttack != null)
            {
                ResetStateFlags();
                return _attackState;
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
                _willPerformBlock = RollForActionChance(enemy.blockLikeHood);
            }

            if (enemy.allowAIToPreformDodge)
            {
                _willPerformDodge = RollForActionChance(enemy.dodgeLikeHood);
            }

            if (enemy.allowAIToPreformParry)
            {
                _willPerformParry = RollForActionChance(enemy.parryLikeHood);
            }


            if (_willPerformBlock)
            {
                BlockingUsingOffHand(enemy);
            }
            if (enemy.currentTarget.isAttacking)
            {
                if (_willPerformDodge)
                {
                    Dodge(enemy);
                }
                if (_willPerformParry && !_hasPerformedParry)
                {
                    ParryCurrentTarget(enemy);
                }
            }
        }

        private bool RollForActionChance(int likeHood)
        {
            int actionChance = Random.Range(0, 100);
            if (actionChance <= likeHood)
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
            _hasRandomDodgeDirection = false;
            _hasPreformedDodge = false;
            _hasPerformedParry = false;

            _randomDestinationSet = false;
            
            _willPerformBlock = false;
            _willPerformDodge = false;
            _willPerformParry = false;
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
                    if (_attackState.currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;

                    if (temporaryScore > randomValue)
                    {
                        _attackState.currentAttack = enemyAttackAction;
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
        private void BlockingUsingOffHand(EnemyManager enemy)
        {
            if(enemy.isBlocking == false)
            {
                if(enemy.allowAIToPreformBlock)
                {
                    enemy.isBlocking = true;
                    enemy.characterWeaponSlotManager.currentItemBeingUsed = enemy.characterWeaponSlotManager.leftWeapon;
                    enemy.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                }
            }
        }
        private void Dodge(EnemyManager enemy)
        {
            if(!_hasPreformedDodge)
            {
                if(!_hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;

                    _hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    _targetDodgeDirection = Quaternion.Euler(enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
                }
                if(enemy.transform.rotation != _targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, _targetDodgeDirection, 1f);
                    enemy.transform.rotation = targetRotation;

                    float targetYRotation = _targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = enemy.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if(rotationDifference <= 5)
                    {
                        _hasPreformedDodge = true;
                        enemy.transform.rotation = _targetDodgeDirection;
                        enemy.enemyLocomotionManager.HandleRoll();
                    }
                }
            }
        }
        private void ParryCurrentTarget(EnemyManager enemy)
        {
            if(enemy.currentTarget.canBeParried)
            {
                if(enemy.distanceFromTarget <= 2)
                {
                    _hasPerformedParry = true;
                    enemy.isParrying = true;
                    enemy.enemyAnimatorManager.PlayTargetAnimation("Parry", true);
                }
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