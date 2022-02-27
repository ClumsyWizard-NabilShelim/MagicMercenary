using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
	protected Animator animator;

	[Header("Base Stats")]
	[SerializeField] protected int damage;
	[SerializeField] private float attackDelay;
	private float currentAttackTime;
	[SerializeField] protected float attackRange;
	[SerializeField] protected Transform origin;
	public Action OnAttack { get; set; }

	protected virtual void Start()
	{
		currentAttackTime = attackDelay;

		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		//Attacking
		if (currentAttackTime <= 0)
		{
			if (PerformAttack())
			{
				currentAttackTime = attackDelay;
				OnAttack?.Invoke();
				Attack();
			}
		}
		else
		{
			currentAttackTime -= Time.deltaTime;
		}
	}

	protected abstract bool PerformAttack();
	protected abstract void Attack();

	//Other
	public void Activate()
	{
		gameObject.SetActive(true);
	}
	public void Deactivate()
	{
		gameObject.SetActive(false);
	}
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		if (origin != null)
			Gizmos.DrawWireSphere(origin.position, attackRange);
	}
}