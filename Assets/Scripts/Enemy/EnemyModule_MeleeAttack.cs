using System.Collections;
using UnityEngine;

public class EnemyModule_MeleeAttack : EnemyModule_Attacking
{
	[SerializeField] private float attackRadius;
	[SerializeField] private LayerMask damageableLayer;
	[SerializeField] private Transform attackOrigin;
	[SerializeField] private int damage;

	[SerializeField] private float timeBetweenAttack;
	private float currentTime;

	protected override void Attack()
	{
		if(currentTime <= 0)
		{
			Collider2D[] cols = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, damageableLayer);

			if(cols != null)
			{
				for (int i = 0; i < cols.Length; i++)
				{
					if (cols[i].CompareTag("Player"))
					{
						animator.SetTrigger("Attack");
						Damageable damageable = cols[i].GetComponent<Damageable>();
						if (damageable != null)
						{
							damageable.Damage(damage);
							currentTime = timeBetweenAttack;
							break;
						}
					}
				}
			}
		}
		else
		{
			currentTime -= Time.deltaTime;
		}
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
	}
}