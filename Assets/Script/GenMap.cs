using System.Collections.Generic;
using pure.ui.image;
using UnityEngine;
using UnityEngine.UI;

public class GenMap : MonoBehaviour
{
    public GameObject prefab;
    public int mapSize = 2;
    public Vector2 offset = new Vector2(0, 0);

    struct CellData
    {
        public int x;
        public int y;
        public GameObject gameObj;
    }

    private Dictionary<(int, int), CellData> hexMap;


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

        hexMap = new Dictionary<(int, int), CellData>();
        int totalRows = mapSize * 2 - 1;
        for (int row = 0; row < totalRows; row++)
        {
            int totalCols = totalRows - Mathf.Abs(row + 1 - mapSize);
            for (int col = 0; col < totalCols; col++)
            {
                GameObject obj = Instantiate(prefab) as GameObject;
                obj.transform.parent = transform;
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.name = "cell" + row + "_" + col;
                var rect = obj.GetComponent<RectTransform>().rect;
                var mid = (totalCols - 1) / 2f;
                obj.transform.localPosition = new Vector3((col - mid) * (rect.width + offset.x), -(row - mapSize + 1f) * (rect.height + offset.y), 0);

                var txt = obj.GetComponentInChildren<Text>();
                txt.text = row + "_" + col;
                var cellData = new CellData {x = row, y = col, gameObj = obj};
                hexMap.Add((row, col), cellData);
            }
        }
    }
}