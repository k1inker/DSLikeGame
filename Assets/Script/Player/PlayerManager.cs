using UnityEngine;

namespace SG
{
    public class PlayerManager : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private PlayerLocomotion _playerLocomotion;
        private Animator _anim;
        private CameraHandler _cameraHandler;
        
        public bool isInteracting;

        [Header("Player flag")]
        public bool isFalling;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isAttack;
        void Start()
        {
            _inputHandler = GetComponent<InputHandler>();
            _anim = GetComponentInChildren<Animator>();
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _cameraHandler = FindObjectOfType<CameraHandler>();
        }
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;

            if (_cameraHandler != null)
            {
                _cameraHandler.FollowTarget(delta);
                _cameraHandler.HandleCameraRotation(delta, _inputHandler.mouseX, _inputHandler.mouseY);
            }
        }
        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = _anim.GetBool("isInteracting");
            canDoCombo = _anim.GetBool("canDoCombo");

            _inputHandler.rollFlag = false;
            _inputHandler.attackFlag = false;

            _inputHandler.TickInput(delta);
            _playerLocomotion.HandelMovement(delta);
            _playerLocomotion.HadleFalling(delta, _playerLocomotion.moveDirection);
            _playerLocomotion.HandleRolling(delta);
            _playerLocomotion.HandleAttackMovement(delta);
        }
        private void LateUpdate()
        {
            _inputHandler.space_Input = false;
            _inputHandler.rb_Input = false;
            _inputHandler.lb_Input = false;
            if (isFalling)
            {
                _playerLocomotion.inAirTimer = _playerLocomotion.inAirTimer + Time.deltaTime;
            }
        }
    }
}