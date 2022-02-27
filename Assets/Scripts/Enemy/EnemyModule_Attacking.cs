using System.Collections;
using UnityEngine;

public abstract class EnemyModule_Attacking : EnemyModule
{
	protected Animator animator;
	private Rigidbody2D rb;
	private EnemyAI enemyAI;
	[SerializeField] private float moveSpeed;
	protected GameObject target;
	[SerializeField] private float attackDistance;

	protected override void Awake()
	{
		base.Awake();
		enemyAI = GetComponent<EnemyAI>();
		animator = GetComponent<Animator>();

		target = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
	}

	public override void OnInitialize()
	{
		enemyMovement.SetData(moveSpeed, 0, OnEndReached, () => { enemyMovement.GetPathTo(target.transform.position); });
	}

	public override void OnUpdate()
	{
		if (target == null)
		{
			enemyAI.SetState(EnemyState.Patrol);
			return;
		}

		if (Vector2.Distance(transform.position, target.transform.position) <= attackDistance)
		{
			rb.velocity = Vector2.zero;
			Attack();
			enemyMovement.RotateLookDirection(target.transform.position - transform.position);
		}
		else
		{
			enemyMovement.Move();
		}
	}

	protected abstract void Attack();

	protected override void OnEndReached()
	{

	}

	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackDistance);
	}
}