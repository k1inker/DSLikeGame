using UnityEngine;
namespace DS
{ 
    public class BossCombatStanceStateHumanoid : CombatStanceStateHumanoid
    {
        [SerializeField] private EnemyAttackAction[] secondPhaseAttacks;
        [SerializeField] private EnemyAttackAction[] enemyAttacks;

        private BossAttackStateHumanoid _bossAttackState;

        public bool hasPhaseShifted;
        protected override void Awake()
        {
            base.Awake();
            _bossAttackState = GetComponent<BossAttackStateHumanoid>();
        }
        public override State Tick(EnemyManager enemy)
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
                DecideCirclingAction(enemy.enemyAnimatorManager);
            }

            HandleRotateTowardsTarget(enemy);
            if (enemy.currentRecoveryTime <= 0 && _bossAttackState.currentAttackAction != null)
            {
                ResetStateFlags();
                return _bossAttackState;
            }
            else
            {
                GetNewAttack(enemy);
            }

            HandleMovement(enemy);
            return this;
        }
        protected override void GetNewAttack(EnemyManager enemy)
        {
            if (hasPhaseShifted)
            {
                ChooseAttackAction(enemy,secondPhaseAttacks);
            }
            else
            {
                ChooseAttackAction(enemy, enemyAttacks);
            }
        }
        private void ChooseAttackAction(EnemyManager enemy, EnemyAttackAction[] enemyAttacks)
        {
            int maxScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];
                if (InRange(enemyAttackAction, enemy.viewableAngle, enemy.distanceFromTarget))
                    maxScore += enemyAttackAction.attackScore;
            }
            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;
            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];
                if (InRange(enemyAttackAction, enemy.viewableAngle, enemy.distanceFromTarget))
                {
                    if (_bossAttackState.currentAttack != null)
                        return;

                    temporaryScore += enemyAttackAction.attackScore;
                    if (temporaryScore > randomValue)
                    {
                        _bossAttackState.currentAttackAction = enemyAttackAction;
                        return;
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