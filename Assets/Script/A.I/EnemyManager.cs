using UnityEngine;
using UnityEngine.AI;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        private EnemyStatsManager _enemyStatsManager;

        [SerializeField] private State currentState;

        public EnemyAnimatorManager enemyAnimatorManager;
        public NavMeshAgent navmeshAgent;
        public CharacterStatsManager currentTarget;
        public Rigidbody enemyRigidbody;

        [Header("A.I Setting")]
        public float detectionRadius = 10;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("A.I Combat Settings")]
        public bool AIPerfomCombos;
        public bool isPhaseShifting;
        public float comboLikeliHood;

        public bool isPerformingAction;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;
        private void Awake()
        {
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            _enemyStatsManager = GetComponent<EnemyStatsManager>();
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

            isUsingLeftHand = enemyAnimatorManager.animator.GetBool("isUsingLeftHand");
            isUsingRightHand = enemyAnimatorManager.animator.GetBool("isUsingRightHand");
            isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
            isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
            enemyAnimatorManager.animator.SetBool("isDead", _enemyStatsManager.isDead);
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
                State nextState = currentState.Tick(this, _enemyStatsManager, enemyAnimatorManager);
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