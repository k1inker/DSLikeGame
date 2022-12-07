using UnityEngine;

namespace DS
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        private EnemyManager _enemyManager;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            _enemyManager = GetComponentInParent<EnemyManager>();
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
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            _enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            _enemyManager.enemyRigidbody.velocity = velocity;
        }
    }
}
