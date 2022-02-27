using System.Collections;
using UnityEngine;

public class EnemyModule_Investigating : EnemyModule
{
	[SerializeField] private float moveSpeed;
	[SerializeField] private float endWaitDelay;

	private EnemyFieldOfView fov;
	private EnemyAI enemyAI;

	protected override void Awake()
	{
		base.Awake();
		fov = GetComponent<EnemyFieldOfView>();
		enemyAI = GetComponent<EnemyAI>();
	}

	public override void OnInitialize()
	{
		enemyMovement.SetData(moveSpeed, endWaitDelay, OnEndReached, () => { enemyMovement.GetPathTo(fov.LastKnowTargetPosition); });
	}

	public override void OnUpdate()
	{
		if ((int)enemyAI.GetEnemyStats().FearLevel != (int)FearLevel.Berserk)
		{
			enemyMovement.Move();
		}
		else
		{
			enemyMovement.CountDownToOnEnd();
			enemyMovement.RotateLookDirection(fov.LastKnowTargetPosition - (Vector2)transform.position);
		}
	}

	protected override void OnEndReached()
	{
		enemyAI.SetState(EnemyState.Patrol);
	}
}