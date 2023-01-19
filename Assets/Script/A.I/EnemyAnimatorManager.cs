using UnityEngine;

namespace DS
{
    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        private EnemyManager _enemy;

        protected override void Awake()
        {
            base.Awake();
            _enemy = GetComponent<EnemyManager>();
        }
        public void InstatiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(_enemy.enemyBossManager.particleFX, bossFXTransform.transform);
        }
        public void PlayWeaponTrailFX()
        {
            _enemy.enemyEffectsManager.PlayWeaponFX(false);
        }
        private void OnAnimatorMove()
        {
            _enemy.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = _enemy.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            _enemy.enemyRigidbody.velocity = velocity;

            if(_enemy.isRotatingWithRootMotion)
            {
                _enemy.transform.rotation *= _enemy.animator.deltaRotation;
            }
        }
    }
}
