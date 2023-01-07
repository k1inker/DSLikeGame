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
            anim = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
            _enemyBossManager = GetComponentInParent<EnemyBossManager>();
            _enemyStats = GetComponentInParent<EnemyStats>();
        }
        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }
        public void StopRotation()
        {
            anim.SetBool("canRotate", false);
        }
        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }
        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }
        public void EnableInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }
        public void DisableInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
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
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _enemyManager.enemyRigidbody.velocity = velocity;

            if(_enemyManager.isRotatingWithRootMotion)
            {
                _enemyManager.transform.rotation *=anim.deltaRotation;
            }
        }
    }
}
