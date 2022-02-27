using System.Collections;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
	[Header("Extra Stats")]
	[SerializeField] private LayerMask damageableLayer;
	[SerializeField] private int maxAttackNumber;

	protected override bool PerformAttack()
	{
		return Input.GetMouseButtonDown(0);
	}

	protected override void Attack()
	{
		animator.SetTrigger("Attack_" + UnityEngine.Random.Range(1, maxAttackNumber + 1).ToString());
		NoiseManager.Instance.Distract();
		AttackComplete();
	}
	private void AttackComplete()
	{ 
		Collider2D damageCollider = Physics2D.OverlapCircle(origin.position, attackRange, damageableLayer);

		if (damageCollider && !damageCollider.CompareTag("Player"))
		{
			Damageable damageable = damageCollider.GetComponent<Damageable>();

			if (damageable != null)
				damageable.Damage(damage);
		}
	}
}