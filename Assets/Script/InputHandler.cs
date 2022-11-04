using Newtonsoft.Json.Bson;
using UnityEngine;

namespace SG
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool rollFlag;
        public bool attackFlag;
        public bool comboFlag;

        public bool space_Input;
        public bool rb_Input;
        public bool lb_Input;

        private PlayerControls _inputActions;
        private PlayerAttacker _playerAttacker;
        private PlayerInvertory _playerInvertory;
        private PlayerManager _playerManager;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private void Awake()
        {
            _playerAttacker = GetComponent<PlayerAttacker>();
            _playerInvertory = GetComponent<PlayerInvertory>();
            _playerManager = GetComponent<PlayerManager>();
        }
        private void OnEnable()
        {
            if(_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.PlayerMovement.Movement.performed += inputActions => _movementInput = inputActions.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
            }

            _inputActions.Enable();
        }
        private void OnDisable()
        {
            _inputActions.Disable();
        }
        
        public void MoveInput(float delta)
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }
        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
        }
        private void HandleRollInput(float delta)
        {
            _inputActions.PlayerActions.Roll.performed += i => space_Input = true;
            if (space_Input)
                rollFlag = true;
        }
        private void HandleAttackInput(float delta)
        {
            _inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            _inputActions.PlayerActions.LB.performed += i => lb_Input = true;
            if(rb_Input)
            {
                if (_playerManager.canDoCombo)
                {
                    attackFlag = true;
                    comboFlag = true;
                    _playerAttacker.HandleWeaponCombo(_playerInvertory.leftWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (_playerManager.isInteracting)
                        return;
                    if (_playerManager.canDoCombo)
                        return;
                    attackFlag = true;
                    _playerAttacker.HandleLightAttack(_playerInvertory.leftWeapon);
                }
            }
            if(lb_Input)
            {
                if (_playerManager.isInteracting)
                    return;
                attackFlag = true;
                _playerAttacker.HandleHeavyAttack(_playerInvertory.leftWeapon);
            }
        }
    }
}
