using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class RandomGenCell : MonoBehaviour {
	[Serializable]
	public struct CellStyle {
		public Color32 color;
		public Vector2 center;

		CellStyle(int size) {
			color = Color.red;
			center = new Vector2(0, 0);
		}
	}

	public CellStyle[] cellStyles;
	public bool[,,] grid;
}