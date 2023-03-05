using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DS
{
    public class ButtonEventTrigger : EventTrigger
    {
        private InputHandler _inputHandler;

        private bool _isAttackButton;
        private bool _hold = false;
        private float _timeHold = 0;

        public Slider attackSlider;
        private void Start()    
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            attackSlider = FindObjectOfType<UIManager>().attackSlider;
        }
        public void FixedUpdate()
        {
            if (_hold)
            {
                _timeHold += Time.deltaTime;
                attackSlider.value = _timeHold;
            }
            else if(!_hold && _isAttackButton)
            {
                _inputHandler.hold_rb_Input = false;
                attackSlider.gameObject.SetActive(false);
                attackSlider.value = 0;
            }

            if(_timeHold > 0.5)
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
            {
                _inputHandler.hold_lb_Input = false;
            }
            if(_inputHandler.hold_rb_Input)
            {
                _inputHandler.hold_rb_Input = false;
            }
            attackSlider.value = 0;
            _timeHold = 0;
            _hold = false;
        }
    }
}
