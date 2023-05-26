using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Game
{
    public class HexCell : MonoBehaviour, IPointerClickHandler
    {
        private int _row = -1;

        public int row
        {
            set
            {
                _row = value;
                GetComponentInChildren<Text>().text = $"{value} {col}";
            }
            get => _row;
        }

        private int _col = -1;

        public int col
        {
            set
            {
                _col = value;
                GetComponentInChildren<Text>().text = $"{row} {value}";
            }
            get => _col;
        }

        public readonly Dictionary<CellDir, HexCell> NearBy = new Dictionary<CellDir, HexCell>();

        private bool _hasCell;

        public bool hasCell
        {
            get => _hasCell;
            set
            {
                _hasCell = value;
                if (value)
                {
                    var graphic = GetComponent<Graphic>();
                    graphic.color = Color.red;
                }
                else
                {
                    GetComponent<Graphic>().color = new Color32(0x3d, 0x3d, 0x3d, 0xFF);
                }
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var str = "current:" + this + this.hasCell + "\n";
            foreach (var hex in NearBy)
            {
                str += $"{hex.Key} " + (hex.Value != null ? $"{hex.Value.row} {hex.Value.col} {hex.Value.hasCell}" : "null") + "\n";
            }

            Debug.Log(str);
            hasCell = !hasCell;

            HexManager.CheckHexCellLine(this);
        }
    }
}