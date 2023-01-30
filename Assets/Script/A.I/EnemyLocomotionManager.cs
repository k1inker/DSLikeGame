using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace DS
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider _characterCollider;
        [SerializeField] private CapsuleCollider _characterCollisionBlockerCollied;
        [SerializeField] private float _rollSpeed = 1f;
        private EnemyManager _enemyManager;
        private Vector3 _normalVector;
        private void Awake()
        {
            _enemyManager = GetComponent<EnemyManager>();
        }
        private void Start()
        {
            Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollied, true);
        }
        public void HandleRoll()
        {
            if (_enemyManager.isInteracting)
                return;
            Vector3 targetDirection = _enemyManager.transform.forward;
            targetDirection.Normalize();
            targetDirection.y = 0;
            targetDirection *= _rollSpeed;
            _enemyManager.enemyRigidbody.velocity = Vector3.ProjectOnPlane(targetDirection, Vector3.zero);
            _enemyManager.enemyAnimatorManager.PlayTargetAnimation("Rolling", true);
        }
    }
}