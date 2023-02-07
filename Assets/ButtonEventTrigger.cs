using UnityEngine;
using UnityEngine.EventSystems;

namespace DS
{
    public class ButtonEventTrigger : EventTrigger
    {
        private bool _isAttackButton;

        private InputHandler _inputHandler;
        private bool _hold = false;
        private float _timeHold = 0;
        private void Start()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
        }
        public void FixedUpdate()
        {
            if (_hold)
                _timeHold += Time.deltaTime;
            if(_timeHold > 0.2)
            {
                _inputHandler.HoldClick(_isAttackButton);
                _hold = false;
            }
        }
        public void OnButtonDown(bool isAttackingButton)
        {
            _timeHold = 0;
            _hold = true;
            _isAttackButton = isAttackingButton;
        }
        public void OnButtonUp()
        {
            if (_timeHold <= 0.2)
            {
                _inputHandler.TapClick(_isAttackButton);
            }
            if (_inputHandler.hold_lb_Input)
                _inputHandler.hold_lb_Input = false;
            _timeHold = 0;
            _hold = false;
        }
    }
}
