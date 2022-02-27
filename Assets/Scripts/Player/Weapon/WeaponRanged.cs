using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRanged : WeaponBase
{
	[Header("Extra Stats")]
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private int startSpreadAngle;
	[SerializeField] private int endSpreadAngle;
	[SerializeField] private float focusSpeed;
	private float spreadAngle;

	[Header("Spread Cone Visualization")]
	[SerializeField] private int meshResolution;
	[SerializeField] private MeshFilter meshFilter;
	private Mesh mesh;
	private bool aimed;

	[Header("Debug")]
	[HideInInspector] public bool DEBUG_ShowFOV;

	protected override void Start()
	{
		base.Start();
		mesh = new Mesh();
		mesh.name = "SpreadMesh";
		meshFilter.mesh = mesh;

		spreadAngle = startSpreadAngle;
	}

	protected override bool PerformAttack()
	{
		if (Input.GetMouseButtonUp(1))
		{
			aimed = false;
			meshFilter.gameObject.SetActive(false);
		}

		if (aimed)
			return false;

		if (Input.GetMouseButtonDown(1))
		{
			animator.SetBool("Pull", true);
			meshFilter.gameObject.SetActive(true);
			spreadAngle = startSpreadAngle;
		}

		if (Input.GetMouseButton(1))
		{
			DrawSpreadCone();

			if (spreadAngle > endSpreadAngle)
				spreadAngle -= focusSpeed * Time.deltaTime;
			else
				spreadAngle = endSpreadAngle;

			if (Input.GetMouseButtonDown(0))
			{
				aimed = true;
				meshFilter.gameObject.SetActive(false);
				NoiseManager.Instance.Distract();
				return true;
			}
		}

		return false;
	}

	protected override void Attack()
	{
		CameraShake.Instance.ShakeObject(0.2f, ShakeMagnitude.Small);
		AudioManager.Instance.PlayAudio("BowAttack");
		animator.SetBool("Pull", false);
		float randomOffset = Random.Range(-spreadAngle / 2, spreadAngle / 2) + transform.eulerAngles.z;
		Projectile projectile = Instantiate(projectilePrefab, origin.position, Quaternion.Euler(0, 0, randomOffset)).GetComponent<Projectile>();
		projectile.Initialize(damage);
	}

	private void DrawSpreadCone()
	{
		int rays = Mathf.RoundToInt(spreadAngle * meshResolution);
		float angleBetweenRays = spreadAngle / rays;

		List<Vector2> viewPoints = new List<Vector2>();

		for (int i = 0; i <= rays; i++)
		{
			float angle = -(spreadAngle / 2) + (angleBetweenRays * i);
			Vector2 dir = DirectionFromAngle(angle, false);
			viewPoints.Add((Vector2)transform.position + dir * attackRange);
		}

		Vector3[] verticies = new Vector3[viewPoints.Count + 1];
		Vector2[] uv = new Vector2[verticies.Length];
		int[] indices = new int[(viewPoints.Count - 1) * 3];

		verticies[0] = Vector2.zero;

		for (int i = 0; i < viewPoints.Count; i++)
		{
			verticies[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < viewPoints.Count - 1)
			{
				indices[i * 3] = 0;
				indices[i * 3 + 1] = i + 1;
				indices[i * 3 + 2] = i + 2;
			}
		}

		mesh.Clear();
		mesh.vertices = verticies;
		mesh.triangles = indices;
		mesh.uv = uv;
		mesh.RecalculateNormals();
	}

	public Vector2 DirectionFromAngle(float angleInDegrees, bool isAngleGlobal)
	{
		if (!isAngleGlobal)
			angleInDegrees -= (transform.eulerAngles.z - 90.0f);

		return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)).normalized;
	}

	//Debug

	public Transform DEBUG_GetOrigin()
	{
		return origin;
	}
	public float DEBUG_GetSpreadAngle()
	{
		return startSpreadAngle;
	}
	public float DEBUG_GetRange()
	{
		return attackRange;
	}
}
