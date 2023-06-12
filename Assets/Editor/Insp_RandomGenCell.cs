using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomGenCell))]
public class Insp_RandomGenCell : Editor {
	private SerializedProperty _cellStylesProp;
	private int buttonSize = 20;
	private GUIStyle defaultStyle;
	private readonly int mapSize = 4;
	private GUIStyle selectStyle;

	private void OnEnable() {
		_cellStylesProp = serializedObject.FindProperty("cellStyles");
		var myScript = target as RandomGenCell;
		var totalRows = mapSize * 2 - 1;

		myScript.grid = new bool[_cellStylesProp.arraySize, totalRows, totalRows];
	}

	public override void OnInspectorGUI() {
		var myScript = target as RandomGenCell;

		serializedObject.Update();

		var totalRows = mapSize * 2 - 1;

		// Initialize the custom style
		if (defaultStyle == null) {
			defaultStyle = new GUIStyle();
			defaultStyle.normal.background = Texture2D.blackTexture;
			defaultStyle.normal.textColor = Color.black;
			defaultStyle.fontSize = 10;
		}

		if (selectStyle == null) {
			selectStyle = new GUIStyle(defaultStyle);
			selectStyle.normal.background = Texture2D.whiteTexture;
			defaultStyle.normal.textColor = Color.red;
		}

		for (var i = 0; i < _cellStylesProp.arraySize; i++) {
			var ele = _cellStylesProp.GetArrayElementAtIndex(i);
			EditorGUILayout.PropertyField(ele, true);
			serializedObject.ApplyModifiedProperties();
			EditorGUILayout.LabelField("Custom Gen Cell");

			// var grid = myScript.grid[i];
			//
			// for (int row = 0; row < totalRows; row++) {
			// 	EditorGUILayout.BeginHorizontal();
			// 	int totalCols = totalRows - Mathf.Abs(row + 1 - mapSize);
			// 	GUILayout.Space(buttonSize / 2 * (totalRows - totalCols));
			// 	for (int col = 0; col < totalCols; col++) {
			// 		// Use the custom style to create a hexagon button
			// 		GUIStyle style = grid[row, col] ? selectStyle : defaultStyle;
			// 		if (GUILayout.Button($"{row} {col}", style, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) {
			// 			// Toggle the value of the grid cell when clicked
			// 			grid[row, col] = !grid[row, col];
			// 		}
			//
			// 		// Shift the next button to the right or left, depending on the row
			// 		GUILayout.Space(3);
			// 	}
			//
			// 	EditorGUILayout.EndHorizontal();
			// }
		}
	}
}