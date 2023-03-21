using UnityEngine;

namespace DS
{
    public class CombatStanceStateHumanoid : State
    {
        private AttackStateHumanoid _attackState;
        protected PursueTargetStateHumanoid pusueTargetState;

        [SerializeField] private ItemBasedAttackAction[] enemyAttacksBasedItem;

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

        private bool _hasBeenRoled;

        private Quaternion _targetDodgeDirection;
        protected virtual void Awake()
        {
            _attackState = GetComponent<AttackStateHumanoid>();
            pusueTargetState = GetComponent<PursueTargetStateHumanoid>();
        }
        public override State Tick(EnemyManager enemy)
        {
            if (enemy.combatStyle == AICombatStyle.swordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else if(enemy.combatStyle == AICombatStyle.heavySword)
            {
                return ProcessHeavySwordCombatStyle(enemy);
            }
            else if(enemy.combatStyle == AICombatStyle.boss)
            {
                return ProcessBossCombatStyle(enemy);
            }
            else
            {
                return this;
            }
        }
        private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
        {

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
                return pusueTargetState;
            }

            //randomiez walking pattern of our A.I so they circle the player 
            if (!_randomDestinationSet)
            {
                _randomDestinationSet = true;
                DecideCirclingAction();
            }

            HandleRotateTowardsTarget(enemy);

            // roll defence chance
            if (!_hasBeenRoled)
            {
                if (enemy.allowAIToPreformBlock)
                {
                    _willPerformBlock = RollForActionChance(enemy.blockLikeHood);
                    _hasBeenRoled = true;
                }
                else if (enemy.allowAIToPreformParry)
                {
                    _willPerformParry = RollForActionChance(enemy.parryLikeHood);
                    _hasBeenRoled = true;
                }
            }

            DefenceAction(enemy);

            if (enemy.currentRecoveryTime <= 0 && _attackState.currentAttack != null)
            {
                ResetStateFlags();

                if(enemy.isBlocking)
                    enemy.isBlocking = false;

                return _attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }

            HandleMovement(enemy);
            return this;
        }
        private State ProcessHeavySwordCombatStyle(EnemyManager enemy)
        {
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);

                return this;
            }
            if (enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return this;
            }
            // if the A.I has gotten to far from it`s target?, return to pursue state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pusueTargetState;
            }

            //randomiez walking pattern of our A.I so they circle the player 
            if (!_randomDestinationSet)
            {
                _randomDestinationSet = true;
                DecideCirclingAction();
            }

            HandleRotateTowardsTarget(enemy);

            //Roll defence action
            if (!_hasBeenRoled)
            {
                if (enemy.allowAIToPreformDodge)
                {
                    _willPerformDodge = RollForActionChance(enemy.dodgeLikeHood);
                    _hasBeenRoled = true;
                }
            }

            DefenceAction(enemy);

            if (enemy.currentRecoveryTime <= 0 && _attackState.currentAttack != null)
            {
                ResetStateFlags();
                return _attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }

            HandleMovement(enemy);
            return this;
        }
        protected virtual State ProcessBossCombatStyle(EnemyManager enemy)
        {
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);

                return this;
            }
            if (enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return this;
            }
            // if the A.I has gotten to far from it`s target?, return to pursue state
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pusueTargetState;
            }

            //randomiez walking pattern of our A.I so they circle the player 
            if (!_randomDestinationSet)
            {
                _randomDestinationSet = true;
                DecideCirclingAction();
            }

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

            HandleMovement(enemy);
            return this;
        }
        private void DefenceAction(EnemyManager enemy)
        {
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
        protected void ResetStateFlags()
        {
            _hasRandomDodgeDirection = false;
            _hasPreformedDodge = false;
            _hasPerformedParry = false;

            _randomDestinationSet = false;
            _hasBeenRoled = false;

            _willPerformBlock = false;
            _willPerformDodge = false;
            _willPerformParry = false;
        }
        protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed); 
        }
        protected virtual void GetNewAttack(EnemyManager enemy)
        {
            int maxScore = 0;
            for (int i = 0; i < enemyAttacksBasedItem.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacksBasedItem[i];
                if (InRange(enemyAttackAction, enemy.viewableAngle, enemy.distanceFromTarget))
                    maxScore += enemyAttackAction.attackScore;
            }
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacksBasedItem.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacksBasedItem[i];
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
        protected void DecideCirclingAction()
        {
            
            WalkAroundTarget();
        }
        protected virtual void WalkAroundTarget()
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
                    enemy.characterWeaponSlotManager.currentItemBeingUsed = enemy.characterWeaponSlotManager.leftWeapon;
                    enemy.characterCombatManager.SetBlockingAbsorptionsFromBlockingWeapon();
                    enemy.characterWeaponSlotManager.leftWeapon.hold_LB_Action.PerformAction(enemy);
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
        protected void HandleMovement(EnemyManager enemy)
        {
            if(enemy.distanceFromTarget <= enemy.stoppingDistance)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", _horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                enemy.animator.SetFloat("Vertical", _verticalMovementValue, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", _horizontalMovementValue, 0.2f, Time.deltaTime);
            }
        }
        private bool InRange(ItemBasedAttackAction enemyAttackAction, float viewableAngle, float distanceFromTarget)
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