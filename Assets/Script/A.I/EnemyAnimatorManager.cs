using UnityEngine;

namespace DS
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyLocomotionManager _enemyLocomotionManager;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            _enemyLocomotionManager = GetComponentInParent<EnemyLocomotionManager>();
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyLocomotionManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _enemyLocomotionManager.enemyRigidbody.velocity = velocity;
        }
    }
}
