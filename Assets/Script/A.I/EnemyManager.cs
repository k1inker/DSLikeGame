using UnityEngine;

namespace SG
{
    public class EnemyManager : MonoBehaviour
    {
        private bool _isPreformingAction;
        private EnemyLocomotionManager _enemyLocomotionManager;
        [Header("A.I Setting")]
        public float detectionRadius = 10;

        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        private void Awake()
        {
            _enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }
        private void Update()
        {
            HandleCurrentAction();
        }
        private void HandleCurrentAction()
        {
            if(_enemyLocomotionManager.currentTarget == null)
            {
                _enemyLocomotionManager.HandleDetection();
            }
        }
    }
}