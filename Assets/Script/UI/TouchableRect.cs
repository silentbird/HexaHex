using UnityEngine;
using UnityEngine.UI;

namespace pure.ui.element {
	[AddComponentMenu("UI/TouchableRect", 34), RequireComponent(typeof(RectTransform))]
	public class TouchableRect : Graphic, ICanvasRaycastFilter {
		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) {
			return true;
		}

		protected virtual void OnPopulateMesh(VertexHelper vh) {
		}

		protected virtual void UpdateGeometry() {
		}
	}
}