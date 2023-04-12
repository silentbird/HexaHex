using System;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexCell : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
    public int x;
    public int y;
    public GameObject gameObj;
    public Dictionary<CellDir, HexCell> nearBy = new Dictionary<CellDir, HexCell>();

    private bool _hasCell = false;

    public bool hasCell
    {
        get { return _hasCell; }
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

    public static CellDir GetOppositeDirection(CellDir CellDir)
    {
        int directionValue = (int) CellDir;
        int oppositeValue = directionValue ^ 3;
        return (CellDir) oppositeValue;
    }

    bool checkHex(HexCell hexCell, CellDir dir)
    {
        if (!hexCell.hasCell)
            return false;
        if (hexCell.nearBy.TryGetValue(dir, out HexCell nextCell))
            return checkHex(nextCell, dir);
        var oppDir = GetOppositeDirection(dir);
        if (hexCell.nearBy.TryGetValue(oppDir, out HexCell oppCell))
            return checkHex(oppCell, oppDir);
        return false;
    }


    public static bool checkHexCellLine(HexCell hexCell)
    {
        CellDir[] dirs = {CellDir.LeftTop, CellDir.RightTop, CellDir.Right};
        foreach (var dir in dirs)
        {
            hexCell.nearBy.TryGetValue(dir, out var cell);
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