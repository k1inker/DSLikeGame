using UnityEngine;

namespace DS
{
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotionManager _enemyLocomotionManager;

        [Header("A.I Setting")]
        public float detectionRadius = 10;

        public bool isPerformingAction;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        private void Awake()
        {
            _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }
        private void FixedUpdate()
        {
            HandleCurrentAction();
        }
        private void HandleCurrentAction()
        {
            if(_enemyLocomotionManager.currentTarget == null)
            {
                _enemyLocomotionManager.HandleDetection();
            }
            else
            {
                _enemyLocomotionManager.HandleMoveToTarget();
            }
        }
    }
}