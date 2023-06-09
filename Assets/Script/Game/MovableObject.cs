using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game {
	public class MovableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
		private Collider2D _newCollider;
		private Collider2D _oldCollider;


		private void Start() {
			GetComponent<RectTransform>().sizeDelta = new Vector2(GameCenter.Size, GameCenter.Size);
		}

		void IDragHandler.OnDrag(PointerEventData evt) {
			if (!Camera.main) return;
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(evt.position);
			transform.position = mousePosition;

			var ray = Camera.main.ScreenPointToRay(new Vector3(evt.position.x, evt.position.y, 0f));
			Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

			var hit = Physics2D.Raycast(ray.origin, ray.direction);
			if (hit.collider != null)
				if (hit.collider != _oldCollider) {
					if (_oldCollider != null) _oldCollider.GetComponent<Graphic>().color = new Color32(0x3d, 0x3d, 0x3d, 0xFF);

					_newCollider = hit.collider;
					_newCollider.GetComponent<Graphic>().color = Color.red;
					_oldCollider = _newCollider;
				}
		}

		void IPointerDownHandler.OnPointerDown(PointerEventData evt) {
		}

		void IPointerUpHandler.OnPointerUp(PointerEventData evt) {
		}
	}
}