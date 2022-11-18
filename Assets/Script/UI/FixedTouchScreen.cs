using UnityEngine;

public class FixedTouchScreen : MonoBehaviour
{
    public Vector2 moveInput = new Vector2();
    private RectTransform _areaTouch;
    [SerializeField] private RectTransform _areaWindow;
    private float _area = 0;
    private Touch _initTouch = new Touch();
    private void Start()
    {
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
                moveInput.x = _initTouch.position.x - touch.position.x;
                moveInput.y = _initTouch.position.y - touch.position.y;
                moveInput.Normalize();
                if (Mathf.Abs(moveInput.x) <= 0.1 || Mathf.Abs(moveInput.y) <= 0.1)
                {
                    moveInput = Vector2.zero;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _initTouch = new Touch();
            }
            else
            {
                moveInput = Vector2.zero;
            }
        }
        if (Input.touches.Length == 0 || (Input.touches[0].position.x < _area && Input.touches.Length == 1))
            moveInput = Vector2.zero;
    }
}
