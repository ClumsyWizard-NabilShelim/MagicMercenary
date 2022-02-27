using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ClumsyWizard;

[CustomEditor(typeof(EnemyFieldOfView))]

public class FOVVisualization : Editor
{
	private EnemyFieldOfView fov;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.BeginHorizontal();
		fov = (EnemyFieldOfView)target;

		if (fov.DEBUG_ShowFOV)
		{
			if (GUILayout.Button("Hide Gizmos"))
			{
				fov.DEBUG_ShowFOV = false;
			}
		}
		else
		{
			if (GUILayout.Button("Show Gizmos"))
			{
				fov.DEBUG_ShowFOV = true;
			}
		}
		GUILayout.EndHorizontal();
	}

	private void OnSceneGUI()
	{
		fov = (EnemyFieldOfView)target;

		if (!fov.DEBUG_ShowFOV || fov.GetLookTransform() == null)
			return;

		Handles.color = Color.black;
		Handles.DrawWireArc(fov.GetLookTransform().position, Vector3.forward, Vector2.right, 360, fov.GetViewRadius(), 3.0f);
		Vector2 viewDirA = MathUtility.GetDirectionFromAngle(-fov.GetViewAngle() / 2.0f, fov.GetLookTransform());
		Vector2 viewDirB = MathUtility.GetDirectionFromAngle(fov.GetViewAngle() / 2.0f, fov.GetLookTransform());

		Handles.color = Color.red;
		Handles.DrawLine(fov.GetLookTransform().position, (Vector2)fov.GetLookTransform().position + viewDirA * fov.GetViewRadius(), 2.0f);
		Handles.DrawLine(fov.GetLookTransform().position, (Vector2)fov.GetLookTransform().position + viewDirB * fov.GetViewRadius(), 2.0f);
	}
}
