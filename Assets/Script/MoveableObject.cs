using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler
{
    private Vector3 offset;
    private Vector3 screenPoint;
    private Collider2D oldCollider;
    private Collider2D newCollider;

    void IPointerDownHandler.OnPointerDown(PointerEventData evt)
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - evt.worldPosition;
    }

    void IDragHandler.OnDrag(PointerEventData evt)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(evt.position);
        transform.position = mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(evt.position.x, evt.position.y, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
        {
            if (hit.collider != oldCollider)
            {
                if (oldCollider != null)
                {
                    oldCollider.GetComponent<Graphic>().color = new Color32(0x3d, 0x3d, 0x3d, 0xFF);
                }

                newCollider = hit.collider;
                newCollider.GetComponent<Graphic>().color = Color.red;
                oldCollider = newCollider;
            }
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData evt)
    {
    }
}