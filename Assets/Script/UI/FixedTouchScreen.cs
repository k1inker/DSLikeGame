using SG;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchScreen : MonoBehaviour
{
    public Vector2 moveInput = new Vector2();
    private RectTransform _areaTouch;
    private float _area = 0;
    private Touch _initTouch = new Touch();
    private void Start()
    {
        _areaTouch = GetComponent<RectTransform>();
        _area = _areaTouch.rect.width;
    }
    private void FixedUpdate()
    {
        float delta = Time.deltaTime;
        foreach(Touch touch in Input.touches)
        {
            Debug.Log(touch.phase);
            if (touch.position.x < _area)
                continue;
            if(touch.phase == TouchPhase.Began)
            {
                _initTouch = touch;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                moveInput.x = _initTouch.position.x- touch.position.x;
                moveInput.y = _initTouch.position.y - touch.position.y;
                moveInput.Normalize();
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
