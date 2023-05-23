using System;
using System.Collections.Generic;
using Script;

public static class HexManager {
	private static CellDir GetOppositeDirection(CellDir cellDir) {
		var memberInfo = typeof(CellDir).GetMember(cellDir.ToString())[0];
		var attributes = memberInfo.GetCustomAttributes(typeof(OppositeDirectionAttribute), false);
		if (attributes.Length == 1 && attributes[0] is OppositeDirectionAttribute oppositeAttribute) {
			return oppositeAttribute.OppositeDirection;
		}

		throw new ArgumentException($"Invalid direction: {cellDir}");
	}

	private static bool checkHex(HexCell hexCell, CellDir dir, ref List<HexCell> line) {
		if (!hexCell.hasCell)
			return false;

		line.Add(hexCell);

		if (!hexCell.nearBy.TryGetValue(dir, out HexCell nextCell) || checkHex(nextCell, dir, ref line))
			return true;
		return false;
	}


	public static void checkHexCellLine(HexCell hexCell) {
		CellDir[]     dirs = {CellDir.LeftTop, CellDir.RightTop, CellDir.Right};
		List<HexCell> line = new List<HexCell>();
		foreach (var dir in dirs) {
			if (!checkHex(hexCell, dir, ref line)) {
				line.Clear();
				line.Add(hexCell);
				continue;
			}

			var oppDir = GetOppositeDirection(dir);
			if (hexCell.nearBy.TryGetValue(oppDir, out HexCell oppCell) && !checkHex(oppCell, oppDir, ref line)) {
				line.Clear();
				line.Add(hexCell);
				continue;
			}
		}

		if (line.Count <= 1) {
			line.Clear();
			return;
		}

		foreach (var hex in line) {
			hex.hasCell = false;
		}
	}
}