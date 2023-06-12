using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace mono.ui {
	public class UnsafeArea : UIBehaviour, ILayoutElement {
		public enum OccupySide {
			Left,
			Right,
			Up,
		}

		public OccupySide side = OccupySide.Left;

		private Canvas _canvas;

		private Vector2 _prefer;

		public Canvas canvas {
			get {
				if (_canvas == null) cache_canvas();
				return _canvas;
			}
		}

		void ILayoutElement.CalculateLayoutInputHorizontal() {
			var screen = Screen.safeArea;
			var root = canvas;
			if (!root) return;
			if (!root.isRootCanvas) root = root.rootCanvas;
			var c = root.renderMode != RenderMode.ScreenSpaceOverlay
				? root.worldCamera
				: null;
			var rect = root.GetComponent<RectTransform>();

			switch (side) {
				case OccupySide.Up:
					RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
						new Vector2(0, 0),
						c,
						out var a_min);
					RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
						screen.position,
						c,
						out var b_min);
					_prefer.y = b_min.y - a_min.y;

					break;
				case OccupySide.Left:
					RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
						new Vector2(0, 0),
						c,
						out var f_min);
					RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
						screen.position,
						c,
						out var s_min);
					_prefer.x = s_min.x - f_min.x;
					break;
				case OccupySide.Right:
					var size = new Vector2(Screen.width, Screen.height);
					RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
						size,
						c,
						out var f_max);
					RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,
						screen.max,
						c,
						out var s_max);
					_prefer.x = f_max.x - s_max.x;
					break;
			}
		}

		void ILayoutElement.CalculateLayoutInputVertical() {
		}

		float ILayoutElement.minWidth => _prefer.x;
		float ILayoutElement.preferredWidth => _prefer.x;
		float ILayoutElement.flexibleWidth => -1;
		float ILayoutElement.minHeight => _prefer.y;
		float ILayoutElement.preferredHeight => _prefer.y;
		float ILayoutElement.flexibleHeight => -1;
		int ILayoutElement.layoutPriority => 0;

		private void cache_canvas() {
			var list = ListPool<Canvas>.Get();
			gameObject.GetComponentsInParent(false, list);
			if (list.Count > 0) {
				// Find the first active and enabled canvas.
				for (var i = 0; i < list.Count; ++i)
					if (list[i].isActiveAndEnabled) {
						_canvas = list[i];
						break;
					}
			}
			else {
				_canvas = null;
			}

			ListPool<Canvas>.Release(list);
		}
	}
}