using UnityEngine;

namespace DS
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager _enemyManager;
        private EnemyBossManager _enemyBossManager;
        private EnemyStats _enemyStats;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
            _enemyBossManager = GetComponentInParent<EnemyBossManager>();
            _enemyStats = GetComponentInParent<EnemyStats>();
        }
        public void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }
        public void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }
        public void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }
        public void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }
        public void EnableInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }
        public void DisableInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }
        public void InstatiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(_enemyBossManager.particleFX, bossFXTransform.transform);
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _enemyManager.enemyRigidbody.velocity = velocity;

            if(_enemyManager.isRotatingWithRootMotion)
            {
                _enemyManager.transform.rotation *=animator.deltaRotation;
            }
        }
    }
}
