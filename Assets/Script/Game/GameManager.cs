using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class GameManager : MonoBehaviour {
		public GameObject prefab;

		public int mapSize = 2;
		public Vector2 offset = new(0, 0);

		private Dictionary<(int, int), HexCell> _hexMap;

		private void Awake() {
			GenerateHexMap();
		}

		private void GenerateHexMap() {
			foreach (Transform child in transform)
				if (Application.isPlaying)
					Destroy(child);
				else
					DestroyImmediate(child);

			_hexMap = new Dictionary<(int, int), HexCell>();
			var totalRows = mapSize * 2 - 1;

			GameCenter.Size = (float)Math.Round(Screen.width / totalRows * 0.9f, 1);

			for (var row = 0; row < totalRows; row++) {
				var totalCols = totalRows - Mathf.Abs(row + 1 - mapSize);
				for (var col = 0; col < totalCols; col++) {
					var obj = Instantiate(prefab, transform, false);
					obj.transform.localScale = new Vector3(1, 1, 1);
					obj.transform.name = "cell" + row + "_" + col;

					obj.GetComponent<HexCell>().Size = GameCenter.Size;
					var rect = obj.GetComponent<RectTransform>().rect;
					var mid = (totalCols - 1) / 2f;
					obj.transform.localPosition = new Vector3((col - mid) * (rect.width + offset.x), -(row - mapSize + 1f) * (rect.height + offset.y), 0);

					var txt = obj.GetComponentInChildren<Text>();
					txt.text = row + "_" + col;
					var hexCell = obj.GetComponentInChildren<HexCell>();
					hexCell.row = row;
					hexCell.col = col;
					_hexMap.Add((row, col), hexCell);
				}
			}

			foreach (var cell in _hexMap) {
				var row = cell.Key.Item1;
				var col = cell.Key.Item2;
				if (_hexMap.ContainsKey((row, col + 1))) cell.Value.NearBy[CellDir.Right] = _hexMap[(row, col + 1)];
				if (_hexMap.ContainsKey((row, col - 1))) cell.Value.NearBy[CellDir.Left] = _hexMap[(row, col - 1)];
				if (row < mapSize - 1) {
					if (_hexMap.ContainsKey((row - 1, col - 1))) cell.Value.NearBy[CellDir.LeftTop] = _hexMap[(row - 1, col - 1)];
					if (_hexMap.ContainsKey((row - 1, col))) cell.Value.NearBy[CellDir.RightTop] = _hexMap[(row - 1, col)];
					if (_hexMap.ContainsKey((row + 1, col))) cell.Value.NearBy[CellDir.LeftBottom] = _hexMap[(row + 1, col)];
					if (_hexMap.ContainsKey((row + 1, col + 1))) cell.Value.NearBy[CellDir.RightBottom] = _hexMap[(row + 1, col + 1)];
				}
				else if (row == mapSize - 1) {
					if (_hexMap.ContainsKey((row - 1, col - 1))) cell.Value.NearBy[CellDir.LeftTop] = _hexMap[(row - 1, col - 1)];
					if (_hexMap.ContainsKey((row - 1, col))) cell.Value.NearBy[CellDir.RightTop] = _hexMap[(row - 1, col)];
					if (_hexMap.ContainsKey((row + 1, col - 1))) cell.Value.NearBy[CellDir.LeftBottom] = _hexMap[(row + 1, col - 1)];
					if (_hexMap.ContainsKey((row + 1, col))) cell.Value.NearBy[CellDir.RightBottom] = _hexMap[(row + 1, col)];
				}
				else {
					if (_hexMap.ContainsKey((row - 1, col))) cell.Value.NearBy[CellDir.LeftTop] = _hexMap[(row - 1, col)];
					if (_hexMap.ContainsKey((row - 1, col + 1))) cell.Value.NearBy[CellDir.RightTop] = _hexMap[(row - 1, col + 1)];
					if (_hexMap.ContainsKey((row + 1, col - 1))) cell.Value.NearBy[CellDir.LeftBottom] = _hexMap[(row + 1, col - 1)];
					if (_hexMap.ContainsKey((row + 1, col))) cell.Value.NearBy[CellDir.RightBottom] = _hexMap[(row + 1, col)];
				}
			}
		}
	}
}