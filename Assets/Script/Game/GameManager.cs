using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public GameObject cellPrefab;
        public int mapSize = 2;
        public Vector2 offset = new Vector2(0, 0);

        private Dictionary<(int, int), HexCell> _hexMap;


        private void Awake()
        {
            GenerateHexMap();
        }

        private void GenerateHexMap()
        {
            foreach (Transform child in transform)
            {
                if (Application.isPlaying)
                    Destroy(child);
                else
                    DestroyImmediate(child);
            }

            _hexMap = new Dictionary<(int, int), HexCell>();
            int totalRows = mapSize * 2 - 1;

            GameCenter.size = this.GetComponent<RectTransform>().rect.width / totalRows * 0.8f;


            for (int row = 0; row < totalRows; row++)
            {
                int totalCols = totalRows - Mathf.Abs(row + 1 - mapSize);
                for (int col = 0; col < totalCols; col++)
                {
                    GameObject obj = Instantiate(cellPrefab, transform, true);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.transform.name = "cell" + row + "_" + col;
                    var rect = obj.GetComponent<RectTransform>().rect;
                    rect = new Rect(0, 0, GameCenter.size, GameCenter.size);
                    var mid = (totalCols - 1) / 2f;
                    obj.transform.localPosition = new Vector3((col - mid) * (rect.width + offset.x), -(row - mapSize + 1f) * (rect.height + offset.y), 0);

                    var txt = obj.GetComponentInChildren<Text>();
                    txt.text = row + "_" + col;
                    var hexCell = obj.GetComponentInChildren<HexCell>();
                    hexCell.x = row;
                    hexCell.y = col;
                    _hexMap.Add((row, col), hexCell);
                }
            }

            foreach (var cell in _hexMap)
            {
                var row = cell.Key.Item1;
                var col = cell.Key.Item2;
                cell.Value.NearBy[CellDir.LeftTop] = _hexMap.ContainsKey((row - 1, col - 1)) ? _hexMap[(row - 1, col - 1)] : null;
                cell.Value.NearBy[CellDir.RightTop] = _hexMap.ContainsKey((row - 1, col)) ? _hexMap[(row - 1, col)] : null;
                cell.Value.NearBy[CellDir.Right] = _hexMap.ContainsKey((row, col + 1)) ? _hexMap[(row, col + 1)] : null;
                cell.Value.NearBy[CellDir.RightBottom] = _hexMap.ContainsKey((row + 1, col)) ? _hexMap[(row + 1, col)] : null;
                cell.Value.NearBy[CellDir.LeftBottom] = _hexMap.ContainsKey((row + 1, col - 1)) ? _hexMap[(row + 1, col - 1)] : null;
                cell.Value.NearBy[CellDir.Left] = _hexMap.ContainsKey((row, col - 1)) ? _hexMap[(row, col - 1)] : null;
            }
        }
    }
}