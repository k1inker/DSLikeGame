using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DS
{
    public class ButtonEventTrigger : EventTrigger
    {
        private static float timeHoldAttack = 0.4f;
        private static float timeHoldBlock = 0.1f;

        private InputHandler _inputHandler;
        private Slider _attackSlider;

        private bool _isAttackButton;
        private bool _hold = false;
        private float _timeHold = 0;
        private float _holdTime = 0;
        private void Awake()    
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            _attackSlider = FindObjectOfType<UIManager>().attackSlider;
        }
        public void FixedUpdate()
        {
            if (!_hold)
                return;

            _timeHold += Time.deltaTime;
            _attackSlider.value = _timeHold;

            if(_timeHold >= 0.1 && _isAttackButton)
            {
                _attackSlider.gameObject.SetActive(true);
            }

            if(_timeHold >= _holdTime)
            {
                _inputHandler.HoldClick(_isAttackButton);
                if (_isAttackButton)
                {
                    _attackSlider.gameObject.SetActive(false);
                    _attackSlider.value = 0;
                    _hold = false;
                }
            }
        }
        public void OnButtonDown(bool isAttackingButton)
        {
            _timeHold = 0;
            _hold = true;
            _isAttackButton = isAttackingButton;
            if(isAttackingButton)
            {
                _holdTime = timeHoldAttack;
            }
            else
            {
                _holdTime = timeHoldBlock;
            }
        }
        public void OnButtonUp()
        {
            if (_timeHold <= 0.1)
            {
                _inputHandler.TapClick(_isAttackButton);
            }
            if (_inputHandler.hold_lb_Input)
            {
                _inputHandler.hold_lb_Input = false;
            }

            _attackSlider.value = 0;
            _timeHold = 0;
            _hold = false;

        }
    }
}
