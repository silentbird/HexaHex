using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game {
	public class HexCell : MonoBehaviour, IPointerClickHandler {
		public readonly Dictionary<CellDir, HexCell> NearBy = new();

		private int _col = -1;

		private bool _hasCell;
		private int _row = -1;
		private float _size;

		public int row {
			set {
				_row = value;
				GetComponentInChildren<Text>().text = $"{value} {col}";
			}
			get => _row;
		}

		public int col {
			set {
				_col = value;
				GetComponentInChildren<Text>().text = $"{row} {value}";
			}
			get => _col;
		}

		public bool hasCell {
			get => _hasCell;
			set {
				_hasCell = value;
				if (value) {
					var graphic = GetComponent<Graphic>();
					graphic.color = Color.red;
				}
				else {
					GetComponent<Graphic>().color = new Color32(0x3d, 0x3d, 0x3d, 0xFF);
				}
			}
		}

		public float Size {
			get => _size;
			set {
				_size = value;
				setColliderPoints(_size);
				GetComponent<RectTransform>().sizeDelta = new Vector2(_size, _size);
			}
		}


		void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
			var str = "current:" + this + hasCell + "\n";
			foreach (var hex in NearBy) str += $"{hex.Key} " + (hex.Value != null ? $"{hex.Value.row} {hex.Value.col} {hex.Value.hasCell}" : "null") + "\n";

			Debug.Log(str);
			hasCell = !hasCell;

			HexManager.CheckHexCellLine(this);
		}

		private void setColliderPoints(float size) {
			var _polygonCollider2D = GetComponent<PolygonCollider2D>();
			var radius = size / 2;
			Vector2[] vertices = {
				new(0f, radius),
				new(radius * Mathf.Sqrt(3) / 2, radius / 2),
				new(radius * Mathf.Sqrt(3) / 2, -radius / 2),
				new(0f, -radius),
				new(-radius * Mathf.Sqrt(3) / 2, -radius / 2),
				new(-radius * Mathf.Sqrt(3) / 2, radius / 2)
			};
			_polygonCollider2D.points = vertices;
		}
	}
}