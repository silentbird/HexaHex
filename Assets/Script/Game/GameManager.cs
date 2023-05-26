using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Game {
	public class GameManager : MonoBehaviour {
		public GameObject prefab;

		public int mapSize = 2;
		public Vector2 offset = new Vector2(0, 0);

		private Dictionary<(int, int), HexCell> _hexMap;

		private void Awake() {
			GenerateHexMap();
		}

		private void GenerateHexMap() {
			foreach (Transform child in transform) {
				if (Application.isPlaying)
					Destroy(child);
				else
					DestroyImmediate(child);
			}

			_hexMap = new Dictionary<(int, int), HexCell>();
			int totalRows = mapSize * 2 - 1;

			GameCenter.Size = (float)Math.Round(Screen.width / totalRows * 0.8f, 1);

			for (int row = 0; row < totalRows; row++) {
				int totalCols = totalRows - Mathf.Abs(row + 1 - mapSize);
				for (int col = 0; col < totalCols; col++) {
					GameObject obj = Instantiate(prefab, transform, false);
					obj.transform.localScale = new Vector3(1, 1, 1);
					obj.transform.name = "cell" + row + "_" + col;

					obj.GetComponent<HexCell>().Size = GameCenter.Size;
					Rect rect = obj.GetComponent<RectTransform>().rect;
					float mid = (totalCols - 1) / 2f;
					obj.transform.localPosition = new Vector3((col - mid) * (rect.width + offset.x), -(row - mid) * (rect.height + offset.y), 0);

					Text txt = obj.GetComponentInChildren<Text>();
					txt.text = row + "_" + col;
					HexCell hexCell = obj.GetComponentInChildren<HexCell>();
					hexCell.row = row;
					hexCell.col = col;
					_hexMap.Add((row, col), hexCell);
				}
			}

			foreach (KeyValuePair<(int, int), HexCell> cell in _hexMap) {
				int row = cell.Key.Item1;
				int col = cell.Key.Item2;
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