using UnityEngine;

namespace DS
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager _enemyManager;
        private EnemyBossManager _enemyBossManager;
        protected override void Awake()
        {
            base.Awake();
            _enemyManager = GetComponent<EnemyManager>();
            _enemyBossManager = GetComponent<EnemyBossManager>();
        }
        public void InstatiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(_enemyBossManager.particleFX, bossFXTransform.transform);
        }
        private void OnAnimatorMove()
        {
            _enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            _enemyManager.enemyRigidbody.velocity = velocity;

            if(_enemyManager.isRotatingWithRootMotion)
            {
                _enemyManager.transform.rotation *=animator.deltaRotation;
            }
        }
    }
}
