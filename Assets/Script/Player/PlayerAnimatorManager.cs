using TMPro;
using UnityEngine;

namespace DS
{
    public class PlayerAnimatorManager : AnimatorManager
    {

        private PlayerLocomotionManager _playerLocomotionManager;
        private int _vertical;
        private int _horizontal;

        public void Initialize()
        {
            animator = GetComponent<Animator>();
            _playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
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

            animator.SetFloat(_vertical, valueVertical, 0.1f, Time.deltaTime);
            animator.SetFloat(_horizontal, valueHorizontal, 0.1f, Time.deltaTime);
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
        private void OnAnimatorMove()
        {
            if (animator.GetBool("isInteracting") && !animator.GetBool("rootPosit"))
            {
                return;
            }

            _playerLocomotionManager.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / Time.deltaTime;
            _playerLocomotionManager.rigidbody.velocity = velocity;
        }
    }
}