using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRingGadget : ElementalGadget
{
	[SerializeField] private int damage;
	[SerializeField] private LayerMask damageableLayer;
	[SerializeField] private GameObject fireEffect;
	private bool effectSpawned;

	protected override void UseGadget()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ItemData.effectRange, damageableLayer);

		if (useGadget)
		{
			useGadget = false;
			AudioManager.Instance.PlayAudio("GadgetRing");
			Instantiate(spawnPrefab, transform.position, Quaternion.identity).GetComponent<GadgetEffect>().Initialize(effectArea.transform.localScale);
		}

		if (cols != null)
		{
			for (int i = 0; i < cols.Length; i++)
			{
				Damageable damageable = cols[i].GetComponent<Damageable>();
				if (damageable == null)
					continue;

				NoiseManager.Instance.Distract();
				Instantiate(fireEffect, cols[i].transform.position, Quaternion.identity);
				damageable.Damage(damage);
			}

			//TODO: Animations and particle effects
		}

		Destroy(gameObject);
	}
}
