using UnityEngine;

namespace DS
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider _characterCollider;
        [SerializeField] private CapsuleCollider _characterCollisionBlockerCollied;
        private void Start()
        {
            Physics.IgnoreCollision(_characterCollider, _characterCollisionBlockerCollied, true);
        }
    }
}