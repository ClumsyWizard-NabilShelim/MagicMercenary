using System.Collections;
using UnityEngine;
using ClumsyWizard;

public class DistractNearbyEntity : MonoBehaviour
{
	public bool Trigger { private get; set; }

	private void Update()
	{
		if (!Trigger)
			return;

		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, MathUtility.ConvertTransformScaleToPhysicsScale(transform.localScale).x);

		if (cols != null)
		{
			for (int i = 0; i < cols.Length; i++)
			{
				if (!MathUtility.IsPointInsideOval(transform, cols[i].transform.position, false))
					continue;

				IDistractable distractable = cols[i].GetComponent<IDistractable>();

				if (distractable != null)
					distractable.Distract(transform.position);

				Trigger = false;
				NoiseManager.Instance.CloseNoiseArea();
			}
		}
		else
		{
			Debug.Log("Nothing to distract");
			Trigger = false;
			NoiseManager.Instance.CloseNoiseArea();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 0.42f);
	}
}