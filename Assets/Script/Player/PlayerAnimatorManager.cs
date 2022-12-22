using TMPro;
using UnityEngine;

namespace DS
{
    public class PlayerAnimatorManager : AnimatorManager
    {

        private PlayerLocomotion _playerLocomotion;
        private int _vertical;
        private int _horizontal;

        public void Initialize()
        {
            anim = GetComponent<Animator>();
            _playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            _vertical = Animator.StringToHash("Vertical");
            _horizontal = Animator.StringToHash("Horizontal");
        }
        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical
            float valueVertical = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f) valueVertical = 0.5f;
            else if (verticalMovement > 0.55f) valueVertical = 1;
            else if (verticalMovement < 0 && verticalMovement > -0.55f) valueVertical = -0.5f;
            else if (verticalMovement < -0.55f) valueVertical = -1;
            else valueVertical = 0;
            #endregion

            #region Horizontal
            float valueHorizontal = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f) valueHorizontal = 0.5f;
            else if (horizontalMovement > 0.55f) valueHorizontal = 1;
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f) valueHorizontal = -0.5f;
            else if (horizontalMovement < -0.55f) valueHorizontal = -1;
            else valueHorizontal = 0;
            #endregion

            anim.SetFloat(_vertical, valueVertical, 0.1f, Time.deltaTime);
            anim.SetFloat(_horizontal, valueHorizontal, 0.1f, Time.deltaTime);
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
            if (anim.GetBool("isInteracting") && !anim.GetBool("rootPosit"))
            {
                return;
            }

            _playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            _playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}