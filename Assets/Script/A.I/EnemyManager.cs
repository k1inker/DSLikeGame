using System;
using UnityEngine;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotionManager _enemyLocomotionManager;
        private EnemyAnimatorManager _enemyAnimatorManager;
        private EnemyStats _enemyStats;

        [SerializeField] private State currentState;

        public CharacterStats currentTarget;
        [Header("A.I Setting")]
        public float detectionRadius = 10;

        public bool isPerformingAction;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;
        private void Awake()
        {
            _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            _enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            _enemyStats = GetComponent<EnemyStats>();
        }
        private void Update()
        {
            HandleRecoveryTimer();
        }
        private void FixedUpdate()
        {
            HandleStateMachine();
        }
        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, _enemyStats, _enemyAnimatorManager);
                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State nextState)
        {
            currentState = nextState;
        }

        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }
            if(isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
        #region Attack
        //private void AttackTarget()
        //{
        //    if (isPerformingAction)
        //        return;

        //    if(currentAttack == null)
        //    {
        //        GetNewAttack();
        //    }
        //    else
        //    {
        //        isPerformingAction = true;
        //        currentRecoveryTime = currentAttack.recoveryTime;
        //        _enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation,true);
        //        currentAttack = null;
        //    }
        //}
        //private void GetNewAttack()
        //{
        //    Vector3 targetDirection = _enemyLocomotionManager.currentTarget.transform.position - transform.position;
        //    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        //    _enemyLocomotionManager.distanceFromTarget = Vector3.Distance(_enemyLocomotionManager.currentTarget.transform.position, transform.position);

        //    int maxScore = 0;
        //    for(int i = 0; i < enemyAttacks.Length; i++)
        //    {
        //        EnemyAttackAction enemyAttackAction = enemyAttacks[i];
        //        if (InRange(enemyAttackAction, viewableAngle))
        //            maxScore += enemyAttackAction.attackScore;
        //    }
        //    int randomValue = Random.Range(0, maxScore);
        //    int temporaryScore = 0;

        //    for(int i = 0; i < enemyAttacks.Length;i++)
        //    {
        //        EnemyAttackAction enemyAttackAction = enemyAttacks[i];
        //        if(InRange(enemyAttackAction, viewableAngle))
        //        {
        //            if (currentAttack != null)
        //                return;

        //            temporaryScore += enemyAttackAction.attackScore;
                    
        //            if(temporaryScore > randomValue)
        //            {
        //                currentAttack = enemyAttackAction;
        //            }
        //        }
        //    }
        //}
        //private bool InRange(EnemyAttackAction enemyAttackAction, float viewableAngle)
        //{
        //    if(_enemyLocomotionManager.distanceFromTarget <= enemyAttackAction  .maximumDistanceToAttack
        //        && _enemyLocomotionManager.distanceFromTarget >= enemyAttackAction.minimumDistanceToAttack)
        //        if(viewableAngle <= enemyAttackAction.maximumAttackAngle
        //            && viewableAngle >= enemyAttackAction.minimumAttackAngle)
        //            return true;
        //    return false;
        //}
        #endregion
    }
}