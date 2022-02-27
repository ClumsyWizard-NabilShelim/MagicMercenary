using System.Collections;
using UnityEngine;

public class EnemyModule_RangedAttack : EnemyModule_Attacking
{
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform attackOrigin;
	[SerializeField] private int damage;

	[SerializeField] private float timeBetweenAttack;
	private float currentTime;

	protected override void Attack()
	{
		if (currentTime <= 0)
		{
			if (target != null)
			{
				AudioManager.Instance.PlayAudio("BowAttack");
				Projectile projectile = Instantiate(projectilePrefab, attackOrigin.position, attackOrigin.rotation).GetComponent<Projectile>();
				projectile.Initialize(damage);
				currentTime = timeBetweenAttack;
			}
		}
		else
		{
			currentTime -= Time.deltaTime;
		}
	}
}