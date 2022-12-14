using UnityEngine;
using UnityEngine.AI;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        private EnemyStats _enemyStats;

        [SerializeField] private State currentState;

        public EnemyAnimatorManager enemyAnimatorManager;
        public NavMeshAgent navmeshAgent;
        public CharacterStatsManager currentTarget;
        public Rigidbody enemyRigidbody;

        [Header("A.I Setting")]
        public float detectionRadius = 10;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("Combat Flags")]
        public bool canDoCombo;

        [Header("A.I Combat Settings")]
        public bool AIPerfomCombos;
        public bool isPhaseShifting;
        public float comboLikeliHood;

        public bool isPerformingAction;
        public bool isInteracting;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;
        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            _enemyStats = GetComponent<EnemyStats>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            navmeshAgent.enabled = false;
        }
        private void Start()
        {
            enemyRigidbody.isKinematic = false;
        }
        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
            enemyAnimatorManager.animator.SetBool("isDead", _enemyStats.isDead);
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
                State nextState = currentState.Tick(this, _enemyStats, enemyAnimatorManager);
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