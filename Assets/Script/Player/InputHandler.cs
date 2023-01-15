using UnityEngine;

namespace DS
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
        public bool comboFlag;
        public bool lockOnFlag;

        public bool space_Input;
        public bool rb_Input;
        public bool rbh_Input;
        public bool lb_Input;
        public bool a_Input;
        public bool lockOn_Input;
        public bool lockOnRight_Input;
        public bool lockOnLeft_Input;

        private PlayerControls _inputActions;
        private CameraHandler _cameraHandler;
        private PlayerManager _playerManager;
        private BlockingCollider _blockingCollider;
        private PlayerCombatManager _playerCombatManager;
        private PlayerAnimatorManager _playerAnimatorManager;
        private PlayerInvertoryManager _playerInvertoryManager;

        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private FixedTouchScreen _vectorTouch;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _playerCombatManager = GetComponent<PlayerCombatManager>();
            _playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            _blockingCollider = GetComponentInChildren<BlockingCollider>();
            _playerInvertoryManager = GetComponent<PlayerInvertoryManager>();

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
                _inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                //_inputActions.PlayerActions.RBH.performed += i => rbh_Input = true;
                _inputActions.PlayerActions.A.performed += i => a_Input = true;
                _inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                _inputActions.PlayerActions.LockOnTargetLeft.performed += i => lockOnLeft_Input = true;
                _inputActions.PlayerActions.LockOnTargetRight.performed += i => lockOnRight_Input = true;

            }

            _inputActions.Enable();
        }
        private void OnDisable()
        {
            _inputActions.Disable();
        }
        public void HandleMoveInput(float delta)
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
                HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleCombatInput(delta);
            HandleLockOnInput(); 
        }
        private void HandleRollInput(float delta)
        {
            if (space_Input)
                rollFlag = true;
        }

        private void HandleCombatInput(float delta)
        {
            if(rb_Input & _playerInvertoryManager.rightWeapon != null)
            {
                _playerCombatManager.HandleRBAttack();
            }
            if(rbh_Input & _playerInvertoryManager.rightWeapon != null)
            {
                if (_playerManager.isInteracting)
                    return;
                //_playerCombatManager.HandleHeavyAttack(_playerInvertoryManager.rightWeapon);
            }
            if(lb_Input)
            {
                _playerCombatManager.HandleLBAction();
            }
            else
            {
                _playerManager.isBlocking = false;
                if(_blockingCollider.blockingCollider.enabled)
                {
                    _blockingCollider.DisableBlockingCollider();
                }
            }
        }
        private void HandleLockOnInput()
        {
            if(lockOn_Input && !lockOnFlag)
            {
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
            if(lockOnFlag && lockOnLeft_Input)
            {
                lockOnLeft_Input = false;
                _cameraHandler.HandleLockOn();
                if(_cameraHandler.leftLockTarget != null)
                {
                    _cameraHandler.currentLockOnTarget = _cameraHandler.leftLockTarget;
                }
            }
            if(lockOnFlag && lockOnRight_Input)
            {
                lockOnRight_Input = false;
                _cameraHandler.HandleLockOn();
                if(_cameraHandler.rightLockTarget != null)
                {
                    _cameraHandler.currentLockOnTarget = _cameraHandler.rightLockTarget;
                }
            }
            _cameraHandler.SetCameraHeight();
        }

        #region Handle Mobile Inputs
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
        public void SwipeLockOnRight()
        {
            lockOnRight_Input = true;
        }
        public void SwipeLockOnLeft()
        {
            lockOnLeft_Input = true;
        }
        #endregion
    }
}
