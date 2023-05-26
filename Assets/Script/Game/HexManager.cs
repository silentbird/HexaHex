using System;
using System.Collections.Generic;

namespace Game {
	public static class HexManager {
		private static CellDir GetOppositeDirection(CellDir cellDir) {
			var memberInfo = typeof(CellDir).GetMember(cellDir.ToString())[0];
			var attributes = memberInfo.GetCustomAttributes(typeof(OppositeDirectionAttribute), false);
			if (attributes.Length == 1 && attributes[0] is OppositeDirectionAttribute oppositeAttribute) {
				return oppositeAttribute.oppositeDirection;
			}

			throw new ArgumentException($"Invalid direction: {cellDir}");
		}

		private static bool CheckHex(HexCell hexCell, CellDir dir, ref List<HexCell> line) {
			if (!hexCell.hasCell)
				return false;

			line.Add(hexCell);

			if (!hexCell.NearBy.TryGetValue(dir, out HexCell nextCell) || CheckHex(nextCell, dir, ref line))
				return true;
			return false;
		}


		public static void CheckHexCellLine(HexCell hexCell) {
			CellDir[] dirs = { CellDir.LeftTop, CellDir.RightTop, CellDir.Right };
			List<HexCell> line = new List<HexCell>();
			foreach (var dir in dirs) {
				if (!CheckHex(hexCell, dir, ref line)) {
					line.Clear();
					line.Add(hexCell);
					continue;
				}

				var oppDir = GetOppositeDirection(dir);
				if (hexCell.NearBy.TryGetValue(oppDir, out HexCell oppCell) && !CheckHex(oppCell, oppDir, ref line)) {
					line.Clear();
					line.Add(hexCell);
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
}