using UnityEngine;

namespace DS
{
    public class InputHandler : MonoBehaviour
    {

        [SerializeField] private VariableJoystick _joystick;
        [SerializeField] private FixedTouchScreen _vectorTouch;
        
        private PlayerControls _inputActions;
        private PlayerManager _player;

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

        public bool _space_Input;
        public bool tap_rb_Input;
        public bool hold_rbh_Input;
        public bool lb_Input;
        public bool a_Input;
        public bool lockOn_Input;
        public bool lockOnRight_Input;
        public bool lockOnLeft_Input;

        private Vector2 _movementInput;
        private Vector2 _cameraInput;

        private void Awake()
        {
            _player = GetComponent<PlayerManager>();

            _joystick = FindObjectOfType<VariableJoystick>();
        }
        private void OnEnable() 
        {
            if(_inputActions == null)
            {
                _inputActions = new PlayerControls();
                _inputActions.PlayerMovement.Movement.performed += inputActions => _movementInput = inputActions.ReadValue<Vector2>();
                _inputActions.PlayerMovement.Camera.performed += i => _cameraInput = i.ReadValue<Vector2>();
                _inputActions.PlayerActions.Roll.performed += i => _space_Input = true;
                _inputActions.PlayerActions.RB.performed += i => tap_rb_Input = true;

                _inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                _inputActions.PlayerActions.LB.canceled += i => lb_Input = false;

                _inputActions.PlayerActions.RBH.performed += i => hold_rbh_Input = true;
                _inputActions.PlayerActions.RBH.canceled += i => hold_rbh_Input = false;

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
            if (_player.isDead)
                return;
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
            if (_space_Input)
                rollFlag = true;
        }
        private void HandleCombatInput(float delta)
        {
            if(tap_rb_Input & _player.playerWeaponSlotManager.rightWeapon != null)
            {
                _player.playerCombatManager.HandleRBAttack();
            }
            if(hold_rbh_Input & _player.playerWeaponSlotManager.rightWeapon != null)
            {
                if (_player.isInteracting)
                    return;
                _player.playerCombatManager.HandleHeavyAttack(_player.playerWeaponSlotManager.rightWeapon);
            }
            if(lb_Input)
            {
                _player.playerCombatManager.HandleLBAction();
            }
            else
            {
                _player.isBlocking = false;
                if(_player.blockingCollider.blockingCollider.enabled)
                {
                    _player.blockingCollider.DisableBlockingCollider();
                }
            }
        }
        private void HandleLockOnInput()
        {
            if(lockOn_Input && !lockOnFlag)
            {
                lockOn_Input = false;
                _player.cameraHandler.HandleLockOn();
                if(_player.cameraHandler.nearestLockOnTarget != null)
                {
                    _player.cameraHandler.currentLockOnTarget = _player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if(lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                _player.cameraHandler.ClearLockOnTargets();
            }
            if(lockOnFlag && lockOnLeft_Input)
            {
                lockOnLeft_Input = false;
                _player.cameraHandler.HandleLockOn();
                if(_player.cameraHandler.leftLockTarget != null)
                {
                    _player.cameraHandler.currentLockOnTarget = _player.cameraHandler.leftLockTarget;
                }
            }
            if(lockOnFlag && lockOnRight_Input)
            {
                lockOnRight_Input = false;
                _player.cameraHandler.HandleLockOn();
                if(_player.cameraHandler.rightLockTarget != null)
                {
                    _player.cameraHandler.currentLockOnTarget = _player.cameraHandler.rightLockTarget;
                }
            }
            _player.cameraHandler.SetCameraHeight();
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
            tap_rb_Input = true;
        }
        public void ClickRoll()
        {
            _space_Input = true;
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
