using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponRanged))]

public class RangedWeaponSpreadCone : Editor
{
	private WeaponRanged rangedWeapon;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.BeginHorizontal();
		rangedWeapon = (WeaponRanged)target;

		if (rangedWeapon.DEBUG_ShowFOV)
		{
			if (GUILayout.Button("Hide Gizmos"))
			{
				rangedWeapon.DEBUG_ShowFOV = false;
			}
		}
		else
		{
			if (GUILayout.Button("Show Gizmos"))
			{
				rangedWeapon.DEBUG_ShowFOV = true;
			}
		}
		GUILayout.EndHorizontal();
	}

	private void OnSceneGUI()
	{
		rangedWeapon = (WeaponRanged)target;

		if (!rangedWeapon.DEBUG_ShowFOV || rangedWeapon.DEBUG_GetOrigin() == null)
			return;

		Handles.color = Color.black;
		Handles.DrawWireArc(rangedWeapon.DEBUG_GetOrigin().position, Vector3.forward, Vector2.right, 360, rangedWeapon.DEBUG_GetRange(), 3.0f);
		Vector2 viewDirA = rangedWeapon.DirectionFromAngle(-rangedWeapon.DEBUG_GetSpreadAngle() / 2.0f, false);
		Vector2 viewDirB = rangedWeapon.DirectionFromAngle(rangedWeapon.DEBUG_GetSpreadAngle() / 2.0f, false);

		Handles.color = Color.red;
		Handles.DrawLine(rangedWeapon.DEBUG_GetOrigin().position, (Vector2)rangedWeapon.DEBUG_GetOrigin().position + viewDirA * rangedWeapon.DEBUG_GetRange(), 2.0f);
		Handles.DrawLine(rangedWeapon.DEBUG_GetOrigin().position, (Vector2)rangedWeapon.DEBUG_GetOrigin().position + viewDirB * rangedWeapon.DEBUG_GetRange(), 2.0f);
	}
}
