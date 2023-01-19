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
        public CharacterStatsManager currentTarget;
        public EnemyEffectsManager enemyEffectsManager;
        public EnemyAnimatorManager enemyAnimatorManager;

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
        protected override void Awake()
        {
            base.Awake();
            enemyRigidbody = GetComponent<Rigidbody>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
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

            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isPhaseShifting = animator.GetBool("isPhaseShifting");

            animator.SetBool("isDead", isDead);
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