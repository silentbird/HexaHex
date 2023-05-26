using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class HexCell : MonoBehaviour, IPointerClickHandler
    {
        public int x;
        public int y;
        public GameObject gameObj;
        public readonly Dictionary<CellDir, HexCell> NearBy = new Dictionary<CellDir, HexCell>();

        private bool _hasCell;

        private bool hasCell
        {
            get => _hasCell;
            set
            {
                _hasCell = value;
                if (value)
                {
                    GetComponent<Graphic>().color = Color.red;
                }
                else
                {
                    GetComponent<Graphic>().color = new Color32(0x3d, 0x3d, 0x3d, 0xFF);
                }
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Click {x} {y}");
            hasCell = !hasCell;
        }

        private static CellDir GetOppositeDirection(CellDir cellDir)
        {
            int directionValue = (int)cellDir;
            int oppositeValue = directionValue ^ 3;
            return (CellDir)oppositeValue;
        }

        public static bool CheckHex(HexCell hexCell, CellDir dir)
        {
            if (!hexCell.hasCell)
                return false;
            if (hexCell.NearBy.TryGetValue(dir, out HexCell nextCell))
                return CheckHex(nextCell, dir);
            var oppDir = GetOppositeDirection(dir);
            if (hexCell.NearBy.TryGetValue(oppDir, out HexCell oppCell))
                return CheckHex(oppCell, oppDir);
            return false;
        }


        public static bool CheckHexCellLine(HexCell hexCell)
        {
            CellDir[] dirs = { CellDir.LeftTop, CellDir.RightTop, CellDir.Right };
            foreach (var dir in dirs)
            {
                hexCell.NearBy.TryGetValue(dir, out var cell);
                if (!cell)
                {
                }
                else if (cell && !cell.hasCell)
                {
                    return false;
                }
            }

            return true;
        }
    }
}