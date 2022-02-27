using System.Collections;
using UnityEngine;

public class IceRingGadget : ElementalGadget
{
	[SerializeField] private LayerMask freezeableLayer;
	[SerializeField] private GameObject iceCube;

	protected override void UseGadget()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, ItemData.effectRange, freezeableLayer);

		AudioManager.Instance.PlayAudio("GadgetRing");

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
				EnemyMovement moveable = cols[i].GetComponent<EnemyMovement>();
				if (moveable == null)
					continue;

				NoiseManager.Instance.Distract();
				CameraShake.Instance.ShakeObject(0.2f, ShakeMagnitude.Small);
				AudioManager.Instance.PlayAudio("Hit");
				GameObject cube = Instantiate(iceCube, cols[i].transform.position, Quaternion.identity);
				moveable.Freeze(cube);
			}

			//TODO: Animations and particle effects
		}

		Destroy(gameObject);
	}
}