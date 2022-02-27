using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClumsyWizard;

public struct ViewHitInfo
{
	public bool Hit;
	public Vector2 HitPoint;
	public float Distance;
	public float Angle;

	public ViewHitInfo(bool hit, Vector2 hitPoint, float distance, float angle)
	{
		Hit = hit;
		HitPoint = hitPoint;
		Distance = distance;
		Angle = angle;
	}
}

public struct EdgeInfo
{
	public Vector2 PointA;
	public Vector2 PointB;

	public EdgeInfo(Vector2 pointA, Vector2 pointB)
	{
		PointA = pointA;
		PointB = pointB;
	}
}

public class EnemyFieldOfView : MonoBehaviour, IDistractable
{
	private EnemyAI enemyAI;
	public Vector2 LastKnowTargetPosition { get; private set; }

	[SerializeField] private float viewRadius;
	[SerializeField] private float viewAngle;
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private LayerMask targetLayer;
	[SerializeField] private Transform lookTransform;

	[Header("FOV Visualization")]
	[SerializeField] private MeshFilter meshFilter;
	[SerializeField] private float meshResolution;
	[SerializeField] private int edgeResolveIterations;
	[SerializeField] private float edgeDistanceThreshold;
	private Mesh mesh;

	private List<GameObject> deadEnemiesFound = new List<GameObject>();

	[Header("Debug")]
	[HideInInspector] public bool DEBUG_ShowFOV;

	private void Start()
	{
		enemyAI = GetComponent<EnemyAI>();

		mesh = new Mesh();
		mesh.name = "View Mesh";
		meshFilter.mesh = mesh;
	}

	private void Update()
	{
		Collider2D target = Physics2D.OverlapCircle(lookTransform.position, viewRadius, targetLayer);

		if (target != null)
		{
			Vector2 dirToTarget = ((Vector2)target.transform.position - (Vector2)lookTransform.position).normalized;

			if (Vector2.Angle(lookTransform.up, dirToTarget) <= (viewAngle / 2.0f))
			{
				float dstToTarget = Vector2.Distance(lookTransform.position, target.transform.position);
				if (!Physics2D.Raycast(lookTransform.position, dirToTarget, dstToTarget, obstacleLayer))
				{
					if (target.gameObject.CompareTag("DeadEnemy"))
					{
						if (deadEnemiesFound.Contains(target.gameObject))
							return;

						deadEnemiesFound.Add(target.gameObject);

						enemyAI.GetEnemyStats().IncreaseFearLevel();
					}
					else
					{
						enemyAI.GetEnemyStats().SetFearLevel(FearLevel.Afraid);

						LastKnowTargetPosition = target.transform.position;
						enemyAI.TargetInView((2 * viewRadius) / dstToTarget);
					}
				}
			}
		}
	}

	public void Distract(Vector2 distractTo)
	{
		LastKnowTargetPosition = distractTo;
		enemyAI.SetState(EnemyState.Investigate);
		if (enemyAI.GetEnemyStats().FearLevel == FearLevel.None)
			enemyAI.GetEnemyStats().IncreaseFearLevel();
	}

	private void LateUpdate()
	{
		DrawFieldOfView();
	}

	private void DrawFieldOfView()
	{
		float rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float angleBetweenRays = viewAngle / rayCount;

		List<Vector3> viewPoints = new List<Vector3>();
		ViewHitInfo oldViewHitInfo = new ViewHitInfo();

		for (int i = 0; i <= rayCount; i++)
		{
			float angle = - (viewAngle / 2) + (angleBetweenRays * i);
			ViewHitInfo viewHitInfo = ViewCast(angle);

			if (i > 0)
			{
				bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewHitInfo.Distance - viewHitInfo.Distance) > edgeDistanceThreshold;
				if (oldViewHitInfo.Hit != viewHitInfo.Hit || (oldViewHitInfo.Hit && viewHitInfo.Hit && edgeDistanceThresholdExceeded))
				{
					EdgeInfo edgeInfo = FindEdge(oldViewHitInfo, viewHitInfo);

					if (edgeInfo.PointA != Vector2.zero)
						viewPoints.Add(edgeInfo.PointA);
					if (edgeInfo.PointB != Vector2.zero)
						viewPoints.Add(edgeInfo.PointB);
				}
			}

			viewPoints.Add(viewHitInfo.HitPoint);
			oldViewHitInfo = viewHitInfo;
		}

		Vector3[] verticies = new Vector3[viewPoints.Count + 1];
		Vector2[] uv = new Vector2[verticies.Length];
		int[] indices = new int[(viewPoints.Count - 1) * 3];

		verticies[0] = Vector3.zero;

		for (int i = 0; i < viewPoints.Count; i++)
		{
			verticies[i + 1] = lookTransform.InverseTransformPoint(viewPoints[i]);

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

	private ViewHitInfo ViewCast(float angle)
	{
		Vector2 dir = MathUtility.GetDirectionFromAngle(angle, lookTransform);
		RaycastHit2D hit = Physics2D.Raycast(lookTransform.position, dir, viewRadius, obstacleLayer);

		if (hit)
			return new ViewHitInfo(true, hit.point, hit.distance, angle);
		else
			return new ViewHitInfo(false, (Vector2)lookTransform.position + dir * viewRadius, viewRadius, angle);
	}

	private EdgeInfo FindEdge(ViewHitInfo minViewCast, ViewHitInfo maxViewCast)
	{
		float minAngle = minViewCast.Angle;
		float maxAngle = maxViewCast.Angle;
		Vector2 minPoint = Vector2.zero;
		Vector2 maxPoint = Vector2.zero;

		for (int i = 0; i < edgeResolveIterations; i++)
		{
			float angle = (minAngle + maxAngle) / 2.0f;
			ViewHitInfo newHitInfo = ViewCast(angle);
			bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.Distance - newHitInfo.Distance) > edgeDistanceThreshold;

			if (newHitInfo.Hit == minViewCast.Hit || !edgeDistanceThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newHitInfo.HitPoint;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newHitInfo.HitPoint;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}

	public float GetViewRadius()
	{
		return viewRadius;
	}
	public float GetViewAngle()
	{
		return viewAngle;
	}
	public Transform GetLookTransform()
	{
		return lookTransform;
	}
}