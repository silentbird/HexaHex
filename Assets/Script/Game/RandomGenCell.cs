using System;
using UnityEngine;

public class RandomGenCell : MonoBehaviour {
	public CellStyle[] cellStyles;
	public bool[,,] grid;

	[Serializable]
	public struct CellStyle {
		public Color32 color;
		public Vector2 center;

		private CellStyle(int size) {
			color = Color.red;
			center = new Vector2(0, 0);
		}
	}
}