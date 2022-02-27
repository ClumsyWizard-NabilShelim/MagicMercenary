using System.Collections;
using UnityEngine;

public class EnemyModule_Patrolling : EnemyModule
{
	[SerializeField] private float moveSpeed;
	[SerializeField] private float endWaitDelay;

	[SerializeField] private Transform patrolPointHolder;
	private Vector2[] patrolPoints;
	private int currentPatrolPointIndex;
	
	protected override void Awake()
	{
		base.Awake();
		patrolPoints = new Vector2[patrolPointHolder.childCount + 1];

		patrolPoints[patrolPointHolder.childCount] = transform.position;

		for (int i = 0; i < patrolPointHolder.childCount; i++)
		{
			patrolPoints[i] = patrolPointHolder.GetChild(i).position;
		}

		currentPatrolPointIndex = 0;
	}

	public override void OnInitialize()
	{
		enemyMovement.SetData(moveSpeed, endWaitDelay, OnEndReached, null);
		enemyMovement.GetPathTo(patrolPoints[currentPatrolPointIndex]);
	}

	protected override void OnEndReached()
	{
		currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
		enemyMovement.GetPathTo(patrolPoints[currentPatrolPointIndex]);
	}

	public override void OnUpdate()
	{
		enemyMovement.Move();
	}

	private void OnDrawGizmosSelected()
	{
		if (patrolPointHolder == null)
			return;

		Gizmos.color = Color.red;

		if (patrolPoints == null || patrolPoints.Length == 0)
		{
			for (int i = 0; i < patrolPointHolder.childCount; i++)
			{
				Gizmos.DrawSphere(patrolPointHolder.GetChild(i).position, 0.2f);
			}
			return;
		}

		for (int i = 0; i < patrolPoints.Length; i++)
		{
			if (patrolPoints[i] == patrolPoints[currentPatrolPointIndex])
				Gizmos.color = Color.green;
			else
				Gizmos.color = Color.red;

			Gizmos.DrawSphere(patrolPoints[i], 0.2f);
		}
	}
}