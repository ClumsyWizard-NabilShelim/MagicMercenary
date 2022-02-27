using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
	[SerializeField] private List<string> ignoreTags;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float damageRadius;
	[SerializeField] private LayerMask damageLayer;
	[SerializeField] private GameObject destroyEffect;
	private int damage;

	public void Initialize(int damage)
	{
		this.damage = damage;
		GetComponent<Rigidbody2D>().AddForce(transform.right * moveSpeed, ForceMode2D.Impulse);
		Destroy(gameObject, 0.7f);
	}

	private void Update()
	{
		Collider2D col = Physics2D.OverlapCircle(transform.position, damageRadius, damageLayer);

		if (col != null)
		{
			if((ignoreTags == null || ignoreTags.Count == 0))
				Damage(col.GetComponent<Damageable>());
			else if (!ignoreTags.Contains(col.tag))
				Damage(col.GetComponent<Damageable>());
		}
	}

	private void Damage(Damageable damageable)
	{
		if (damageable != null)
			damageable.Damage(damage);

		Hit();
	}

	private void OnDestroy()
	{
		Instantiate(destroyEffect, transform.position, Quaternion.identity);
	}

	private void Hit()
	{
		Instantiate(destroyEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, damageRadius);
	}
}