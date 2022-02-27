using System.Collections;
using UnityEngine;
using ClumsyWizard;

public class RootSpikeGadget : ElementalGadget
{
	[SerializeField] private int damage;
	[SerializeField] private LayerMask damageableLayer;


	protected override void UseGadget()
	{
		Collider2D col = Physics2D.OverlapCircle(transform.position, ItemData.effectRange, damageableLayer);

		if (col != null)
		{
			if (col.CompareTag("Player"))
				return;

			AudioManager.Instance.PlayAudio("BowAttack");
			Projectile projectile = Instantiate(spawnPrefab, transform.position, Quaternion.Euler(0, 0, MathUtility.GetZRotationFromVector(transform.position, col.transform.position))).GetComponent<Projectile>();
			projectile.Initialize(damage);
			NoiseManager.Instance.Distract();
			//TODO: Animations and particle effects
			Destroy(gameObject);
		}
	}
}