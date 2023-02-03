using UnityEngine;
using UnityEngine.AI;

namespace DS
{
    public class EnemyManager : CharacterManager
    {

        [SerializeField] private State currentState;

        public Rigidbody enemyRigidbody;
        public NavMeshAgent navmeshAgent;
        public EnemyBossManager enemyBossManager;
        public EnemyStatsManager enemyStatsManager;
        public CharacterManager currentTarget;
        public EnemyEffectsManager enemyEffectsManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyLocomotionManager enemyLocomotionManager;

        [Header("Level settings")]
        public float chanceSpawn = 100;

        [Header("A.I Setting")]
        public float detectionRadius = 10;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;
        public float stoppingDistance = 1.2f;

        [Header("Advanced A.I settings")]
        public bool allowAIToPreformBlock;
        public int blockLikeHood = 50;
        public bool allowAIToPreformDodge;
        public int dodgeLikeHood = 50;
        public bool allowAIToPreformParry;
        public int parryLikeHood = 50;

        [Header("A.I Combat Settings")]
        public bool AIPerfomCombos;
        public bool isPhaseShifting;
        public float comboLikeliHood;
        public AICombatStyle combatStyle;

        [Header("A.I Target Information")]
        public float distanceFromTarget;
        public float viewableAngle;
        public Vector3 targetDirection;

        public bool isPerformingAction;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;
        public bool isBoss;
        protected override void Awake()
        {
            base.Awake();
            enemyRigidbody = GetComponent<Rigidbody>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();

            currentTarget = FindObjectOfType<PlayerManager>();
        }
        private void Start()
        {
            enemyRigidbody.isKinematic = false;
        }
        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            //isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            //isUsingRightHand = animator.GetBool("isUsingRightHand");
            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isPhaseShifting = animator.GetBool("isPhaseShifting");

            animator.SetBool("isDead", isDead);
            animator.SetBool("isBlocking", isBlocking);

            UpdateInformationFromTarget();
        }

        private void UpdateInformationFromTarget()
        {
            if (currentTarget != null)
            {
                targetDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetDirection, transform.forward);
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            }
        }

        private void LateUpdate()
        {
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;   
        }
        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this);
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

    }
}