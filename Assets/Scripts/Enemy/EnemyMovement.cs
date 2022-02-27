using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	private EnemyFieldOfView fov;
	private Animator animator;
	private Rigidbody2D rb;
	[SerializeField] private SpriteRenderer gfx;

	private Transform lookTransform;
	private Vector2[] path;
	private int wayPointIndex;

	private float moveSpeed;
	private float endWaitDelay;
	private float currentWaitTime;
	[SerializeField] private float stoppingThreshold;
	private Action onEndReached;
	private Vector2 moveDir;
	[SerializeField] private Transform weaponHolder;

	[Header("Elemental Effect: Frozon")]
	[SerializeField] private float frozenTimer;
	private float currentfozenTime;
	private bool frozen;
	private GameObject iceCubeEffect;
	[SerializeField] private GameObject iceCubePopEffect;
	public Action OnWayPointReached { get; set; }


	private void Awake()
	{
		fov = GetComponent<EnemyFieldOfView>();
		lookTransform = fov.GetLookTransform();
		rb = GetComponent<Rigidbody2D>();

		animator = GetComponent<Animator>();
	}

	public void SetData(float moveSpeed, float endWaitDelay, Action onEndReached, Action OnWayPointReached)
	{
		this.moveSpeed = moveSpeed;
		this.endWaitDelay = endWaitDelay;
		currentWaitTime = endWaitDelay;

		this.onEndReached = onEndReached;
		this.OnWayPointReached = OnWayPointReached;
	}

	public void GetPathTo(Vector2 target)
	{
		PathRequestManager.RequestPath(rb.position, target, OnPathFound);
	}

	private void OnPathFound(Vector2[] wayPoints, bool success)
	{
		if(success)
		{
			wayPointIndex = 0;
			path = wayPoints;
		}
	}

	public void Move()
	{
		if(!frozen)
			animator.SetBool("Walk", rb.velocity != Vector2.zero);
		else
			animator.SetBool("Walk", false);

		if (path == null)
			return;

		if (moveDir.y < Mathf.Abs(moveDir.x))
		{
			if (moveDir.x < -0.5f)
				gfx.flipX = true;
			else if (moveDir.x > 0.5f)
				gfx.flipX = false;
		}

		if (wayPointIndex >= path.Length)
		{
			CountDownToOnEnd();
			return;
		}
		else
		{ 
			if(frozen)
			{
				if (currentfozenTime <= 0)
				{
					fov.enabled = true;
					frozen = false;
					Instantiate(iceCubePopEffect, iceCubeEffect.transform.position, Quaternion.identity);
					CameraShake.Instance.ShakeObject(0.2f, ShakeMagnitude.Small);
					AudioManager.Instance.PlayAudio("Hit");
					Destroy(iceCubeEffect);
				}
				else
				{
					fov.enabled = false;
					rb.velocity = Vector2.zero;
					currentfozenTime -= Time.deltaTime;
				}
				return;
			}

			moveDir = (path[wayPointIndex] - rb.position).normalized;
			rb.velocity = moveDir * moveSpeed;
			RotateLookDirection(moveDir);

			float distanceToWayPoint = Vector2.Distance(rb.position, path[wayPointIndex]);

			if (distanceToWayPoint <= stoppingThreshold)
			{
				wayPointIndex++;
				OnWayPointReached?.Invoke();
			}
		}
	}

	public void RotateLookDirection(Vector2 dir)
	{
		float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		weaponHolder.rotation = Quaternion.Lerp(weaponHolder.rotation, Quaternion.Euler(0, 0, rotZ), 5 * Time.deltaTime);
		lookTransform.rotation = Quaternion.Lerp(lookTransform.rotation, Quaternion.Euler(0, 0, rotZ - 90.0f), 10 * Time.deltaTime);
	}

	public void CountDownToOnEnd()
	{
		if (currentWaitTime <= 0)
		{
			onEndReached?.Invoke();
			currentWaitTime = endWaitDelay;
		}
		else
		{
			RotateLookDirection(moveDir);
			rb.velocity = Vector2.zero;
			currentWaitTime -= Time.deltaTime;
		}
	}

	public void Freeze(GameObject iceCubeEffect)
	{
		frozen = true;
		currentfozenTime = frozenTimer;
		this.iceCubeEffect = iceCubeEffect;
	}

	public EnemyFieldOfView GetFOV()
	{
		if(fov == null)
			fov = GetComponent<EnemyFieldOfView>();

		return fov;
	}

	private void OnDrawGizmos()
	{
		if (path == null || path.Length == 0)
			return;

		Gizmos.color = Color.black;
		foreach (Vector2 point in path)
		{
			Gizmos.DrawCube(point, Vector3.one * 0.2f);
		}

		for (int i = 1; i < path.Length; i++)
		{
			Gizmos.DrawLine(path[i - 1], path[i]);
		}
	}
}