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
        public bool tap_lb_Input;
        public bool hold_rb_Input;
        public bool hold_lb_Input;
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
                _inputActions.PlayerActions.LB.performed += i => tap_lb_Input = true;

                _inputActions.PlayerActions.LBH.performed += i => hold_lb_Input = true;
                _inputActions.PlayerActions.LBH.canceled += i => hold_lb_Input = false;

                _inputActions.PlayerActions.RBH.performed += i => hold_rb_Input = true;
                _inputActions.PlayerActions.RBH.canceled += i => hold_rb_Input = false;

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
        public void HandleMoveInput()
        {
            horizontal = _movementInput.x;
            vertical = _movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = _cameraInput.x;
            mouseY = _cameraInput.y;
        }
        public void TickInput()
        {
            if (_player.isDead)
                return;

            if(isJoystick)
                MoveInputJoystick();
            else
                HandleMoveInput();
            HandleRollInput();
            HandleTapRBInput();
            HandleTapLBInput();
            HandleHoldRBInput();
            HandleHoldLBInput();
            HandleLockOnInput(); 
        }
        private void HandleRollInput()
        {
            if (_space_Input)
                rollFlag = true;
        }
        private void HandleTapRBInput()
        {
            if(tap_rb_Input & _player.playerWeaponSlotManager.rightWeapon.tap_RB_Action != null)
            {
                _player.UpdateWichHandCharacterIsUsing(true);
                _player.playerWeaponSlotManager.currentItemBeingUsed = _player.playerWeaponSlotManager.rightWeapon;
                _player.playerWeaponSlotManager.rightWeapon.tap_RB_Action.PerformAction(_player);
            }
        }
        private void HandleHoldRBInput()
        {
            if (hold_rb_Input & _player.playerWeaponSlotManager.rightWeapon.hold_RB_Action != null)
            {
                _player.UpdateWichHandCharacterIsUsing(true);
                _player.playerWeaponSlotManager.currentItemBeingUsed = _player.playerWeaponSlotManager.rightWeapon;
                _player.playerWeaponSlotManager.rightWeapon.hold_RB_Action.PerformAction(_player);
            }
        }
        private void HandleTapLBInput()
        {
            if (tap_lb_Input & _player.playerWeaponSlotManager.leftWeapon.tap_LB_Action != null)
            {
                _player.UpdateWichHandCharacterIsUsing(false);
                _player.playerWeaponSlotManager.currentItemBeingUsed = _player.playerWeaponSlotManager.leftWeapon;
                _player.characterWeaponSlotManager.leftWeapon.tap_LB_Action.PerformAction(_player);
            }
        }
        private void HandleHoldLBInput()
        {
            if (hold_lb_Input & _player.playerWeaponSlotManager.leftWeapon.hold_LB_Action != null)
            {
                _player.UpdateWichHandCharacterIsUsing(false);
                _player.playerWeaponSlotManager.currentItemBeingUsed = _player.playerWeaponSlotManager.leftWeapon;
                _player.characterWeaponSlotManager.leftWeapon.hold_LB_Action.PerformAction(_player);
            }
            else
            {
                if (_player.isBlocking)
                {
                    _player.isBlocking = false;
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
