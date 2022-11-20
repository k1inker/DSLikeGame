using System;
using UnityEngine;

namespace SG
{
    public class FixedTouchScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _areaWindow;

        private InputHandler _inputHandler;
        private RectTransform _areaTouch;
        private Touch _initTouch = new Touch();
        
        private float _area = 0;
        private bool isSwiped = false;

        public Vector2 moveInput = new Vector2();
        private void Start()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            _areaTouch = GetComponent<RectTransform>();
            _area = _areaWindow.rect.size.x - _areaTouch.rect.size.x;
        }
        private void FixedUpdate()
        {
            float delta = Time.deltaTime;
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < _area)
                    continue;
                if (touch.phase == TouchPhase.Began)
                {
                    _initTouch = touch;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (_inputHandler.lockOnFlag && !isSwiped)
                        HandlerLockOn(touch);
                    else
                        FreeAspectCamera(touch);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    _initTouch = new Touch();
                    isSwiped = false;
                }
                else
                {
                    moveInput = Vector2.zero;
                }
            }
            if (Input.touches.Length == 0 || (Input.touches[0].position.x < _area && Input.touches.Length == 1))
            {
                moveInput = Vector2.zero;
                isSwiped = false;
            }
        }

        private void HandlerLockOn(Touch touch)
        {
            moveInput.x = _initTouch.position.x - touch.position.x;
            moveInput.Normalize();
            if (moveInput.x < 0)
            {
                _inputHandler.SwipeLockOnRight();
                isSwiped = true;
            }
            else if (moveInput.x > 0)
            { 
                _inputHandler.SwipeLockOnLeft();
                isSwiped = true;
            }
        }

        private void FreeAspectCamera(Touch touch)
        {
            moveInput.x = _initTouch.position.x - touch.position.x;
            moveInput.y = _initTouch.position.y - touch.position.y;
            moveInput.Normalize();
            if (Mathf.Abs(moveInput.x) <= 0.1 || Mathf.Abs(moveInput.y) <= 0.1)
            {
                moveInput = Vector2.zero;
            }
        }
    }
}