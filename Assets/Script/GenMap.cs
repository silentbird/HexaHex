using System.Collections.Generic;
using pure.ui.image;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class GenMap : MonoBehaviour {
	public GameObject prefab;

	public int     mapSize = 2;
	public Vector2 offset  = new Vector2(0, 0);

	private Dictionary<(int, int), HexCell> hexMap;

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

		hexMap = new Dictionary<(int, int), HexCell>();
		int totalRows = mapSize * 2 - 1;
		for (int row = 0; row < totalRows; row++) {
			int totalCols = totalRows - Mathf.Abs(row + 1 - mapSize);
			for (int col = 0; col < totalCols; col++) {
				GameObject obj = Instantiate(prefab) as GameObject;
				obj.transform.parent     = transform;
				obj.transform.localScale = new Vector3(1, 1, 1);
				obj.transform.name       = "cell" + row + "_" + col;
				var rect = obj.GetComponent<RectTransform>().rect;
				var mid  = (totalCols - 1) / 2f;
				obj.transform.localPosition = new Vector3((col - mid) * (rect.width + offset.x), -(row - mapSize + 1f) * (rect.height + offset.y), 0);
				var txt = obj.GetComponentInChildren<Text>();
				txt.text = row + "_" + col;
				var hexCell = obj.GetComponentInChildren<HexCell>();
				hexCell.row = row;
				hexCell.col = col;
				hexMap.Add((row, col), hexCell);
			}
		}

		foreach (var cell in hexMap) {
			var row                                                                  = cell.Key.Item1;
			var col                                                                  = cell.Key.Item2;
			if (hexMap.ContainsKey((row, col + 1))) cell.Value.nearBy[CellDir.Right] = hexMap[(row, col + 1)];
			if (hexMap.ContainsKey((row, col - 1))) cell.Value.nearBy[CellDir.Left]  = hexMap[(row, col - 1)];
			if (row < mapSize - 1) {
				if (hexMap.ContainsKey((row - 1, col - 1))) cell.Value.nearBy[CellDir.LeftTop]     = hexMap[(row - 1, col - 1)];
				if (hexMap.ContainsKey((row - 1, col))) cell.Value.nearBy[CellDir.RightTop]        = hexMap[(row - 1, col)];
				if (hexMap.ContainsKey((row + 1, col))) cell.Value.nearBy[CellDir.LeftBottom]      = hexMap[(row + 1, col)];
				if (hexMap.ContainsKey((row + 1, col + 1))) cell.Value.nearBy[CellDir.RightBottom] = hexMap[(row + 1, col + 1)];
			}
			else if (row == mapSize - 1) {
				if (hexMap.ContainsKey((row - 1, col - 1))) cell.Value.nearBy[CellDir.LeftTop]    = hexMap[(row - 1, col - 1)];
				if (hexMap.ContainsKey((row - 1, col))) cell.Value.nearBy[CellDir.RightTop]       = hexMap[(row - 1, col)];
				if (hexMap.ContainsKey((row + 1, col - 1))) cell.Value.nearBy[CellDir.LeftBottom] = hexMap[(row + 1, col - 1)];
				if (hexMap.ContainsKey((row + 1, col))) cell.Value.nearBy[CellDir.RightBottom]    = hexMap[(row + 1, col)];
			}
			else {
				if (hexMap.ContainsKey((row - 1, col))) cell.Value.nearBy[CellDir.LeftTop]        = hexMap[(row - 1, col)];
				if (hexMap.ContainsKey((row - 1, col + 1))) cell.Value.nearBy[CellDir.RightTop]   = hexMap[(row - 1, col + 1)];
				if (hexMap.ContainsKey((row + 1, col - 1))) cell.Value.nearBy[CellDir.LeftBottom] = hexMap[(row + 1, col - 1)];
				if (hexMap.ContainsKey((row + 1, col))) cell.Value.nearBy[CellDir.RightBottom]    = hexMap[(row + 1, col)];
			}
		}
	}
}