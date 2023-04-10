using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
{
    void IPointerDownHandler.OnPointerDown(PointerEventData evt)
    {
        if (evt.button == PointerEventData.InputButton.Right)
            return;

        Debug.Log("OnPointerDown");
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
    }
}