using UnityEngine;

namespace SG
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Input system")]
        public bool isJoystick = false;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool rollFlag;
        public bool attackFlag;
        public bool comboFlag;
        public bool lockOnFlag;

        public bool space_Input;
        public bool rb_Input;
        public bool lb_Input;
        public bool a_Input;
        public bool lockOn_Input;    

        private PlayerControls _inputActions;
        private PlayerAttacker _playerAttacker;
        private PlayerInvertory _playerInvertory;
        private PlayerManager _playerManager;
        private CameraHandler _cameraHandler;

        [SerializeField]private VariableJoystick _joystick;
        [SerializeField] private FixedTouchScreen _vectorTouch;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private void Awake()
        {
            _playerAttacker = GetComponent<PlayerAttacker>();
            _playerInvertory = GetComponent<PlayerInvertory>();
            _playerManager = GetComponent<PlayerManager>();
            _joystick = FindObjectOfType<VariableJoystick>();
            _cameraHandler = FindObjectOfType<CameraHandler>();
        }
        private void OnEnable()
        {
            if(_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.PlayerMovement.Movement.performed += inputActions => _movementInput = inputActions.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
                _inputActions.PlayerActions.Roll.performed += i => space_Input = true;
                _inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                _inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                _inputActions.PlayerActions.A.performed += i => a_Input = true;
                _inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
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
            if(isJoystick)
                MoveInputJoystick();
            else
                MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleLockOnInput(); 
        }
        private void HandleRollInput(float delta)
        {
            if (space_Input)
                rollFlag = true;
        }
        private void HandleAttackInput(float delta)
        {
            if(rb_Input & _playerInvertory.rightWeapon != null)
            {
                if (_playerManager.canDoCombo)
                {
                    attackFlag = true;
                    comboFlag = true;
                    _playerAttacker.HandleWeaponCombo(_playerInvertory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    if (_playerManager.isInteracting)
                        return;
                    if (_playerManager.canDoCombo)
                        return;
                    attackFlag = true;
                    _playerAttacker.HandleLightAttack(_playerInvertory.rightWeapon);
                }
            }
            if(lb_Input & _playerInvertory.rightWeapon != null)
            {
                if (_playerManager.isInteracting)
                    return;
                attackFlag = true;
                _playerAttacker.HandleHeavyAttack(_playerInvertory.rightWeapon);
            }
        }
        private void HandleLockOnInput()
        {
            if(lockOn_Input && !lockOnFlag)
            {
                _cameraHandler.ClearLockOnTargets();
                lockOn_Input = false;
                _cameraHandler.HandleLockOn();
                if(_cameraHandler.nearestLockOnTarget != null)
                {
                    _cameraHandler.currentLockOnTarget = _cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if(lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                _cameraHandler.ClearLockOnTargets();
            }
        }
        public void MoveInputJoystick()
        {
            horizontal = _joystick.Horizontal;
            vertical = _joystick.Vertical;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _vectorTouch.moveInput.x;
            mouseY = _vectorTouch.moveInput.y; 
        }
        public void ClickPickUpItem()
        {
            a_Input = true;
        }
        public void ClickAttack()
        {
            rb_Input = true;
        }
        public void ClickRoll()
        {
            space_Input = true;
        }
        public void ClickLockOn()
        {
            lockOn_Input = true;
        }
    }
}
