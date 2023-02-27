using UnityEngine;

namespace DS
{
    public class FixedTouchScreen : MonoBehaviour
    {
        [SerializeField] private RectTransform _areaWindow;
        private RectTransform _areaTouch;
        private float _area = 0;
        private Vector2 _firstPoint;
        private bool _isLockOn = false;
        private InputHandler _inputHandler;

        public bool IsLockOn { set { _isLockOn = value; } }
        public Vector2 moveInput = Vector2.zero;
        public bool isSwiped;
        private void Start()
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            _areaTouch = GetComponent<RectTransform>();
            _area = Screen.width - _areaTouch.rect.size.x;
        }
        void LateUpdate()
        {
            TouchRotation();
        }
        private void TouchRotation()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < _area)
                    continue;
                if (touch.phase == TouchPhase.Began)
                {
                    _firstPoint = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (!_isLockOn)
                    {
                        FreeAspectCamera(touch);
                    }
                    else if (_isLockOn && !isSwiped)
                    {
                        HandlerLockOn(touch);
                    }
                    moveInput.Normalize();
                }
                else
                {
                    isSwiped = false;
                    moveInput = Vector2.zero;
                }
            }
        }
        private void FreeAspectCamera(Touch touch)
        {
            Vector2 secondPoint = touch.position;

            moveInput.x = FilterGyroValues(_firstPoint.x - secondPoint.x);

            moveInput.y = FilterGyroValues(_firstPoint.y - secondPoint.y);
            _firstPoint = secondPoint;
        }

        private float FilterGyroValues(float axis)
        {
            float thresshold = 5f;
            if (axis < -thresshold || axis > thresshold)
            {
                return axis;
            }
            else
            {
                return 0;
            }
        }
        private void HandlerLockOn(Touch touch)
        {
            moveInput.x = _firstPoint.x - touch.position.x;
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
    }
}