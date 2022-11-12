using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchScreen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 touchDistance;
    private Vector2 _pointerOld;
    private int _pointerID;
    public bool _pressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
        _pointerID = eventData.pointerId;
        _pointerOld = eventData.position;
    }
    private void Update()
    {
        if(_pressed)
        {
            if(_pointerID >= 0 && _pointerID < Input.touches.Length)
            {
                touchDistance = Input.touches[_pointerID].position - _pointerOld;
                _pointerOld -= Input.touches[_pointerID].position;
            }
            else
            {
                touchDistance = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - _pointerOld;
                _pointerOld = Input.mousePosition;
            }
        }
        else
        {
            touchDistance = new Vector2();
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed= false;
    }
}
