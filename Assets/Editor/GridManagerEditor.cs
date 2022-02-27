using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
	GridManager gridManager;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.BeginHorizontal();
		gridManager = (GridManager)target;

		if (gridManager.isGridVisible)
		{
			if (GUILayout.Button("Update Grid"))
			{
				gridManager.DEBUG_InitGrid();
			}

			if (GUILayout.Button("Remove Grid"))
			{
				gridManager.isGridVisible = false;
				gridManager.DEBUG_ClearGrid();
			}
		}
		else
		{
			if (GUILayout.Button("Show Grid"))
			{
				gridManager.isGridVisible = true;
				gridManager.DEBUG_InitGrid();
			}
		}

		GUILayout.EndHorizontal();
	}
}
