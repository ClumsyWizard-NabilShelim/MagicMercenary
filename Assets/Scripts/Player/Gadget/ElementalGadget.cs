using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClumsyWizard;

public abstract class ElementalGadget : MonoBehaviour
{
	public GadgetItem ItemData { get; set; }
	[SerializeField] protected GameObject spawnPrefab;
	private PlayerStats playerStats;
	[SerializeField] protected GameObject effectArea;
	private Camera cam;
	protected bool useGadget;

	private void Start()
	{
		playerStats = FindObjectOfType<PlayerStats>();

		GetComponent<ThrowableItem>().TargetReached += () =>
		{
			useGadget = true;
			playerStats.AlterMana(-ItemData.manaCost);
		};

		cam = Camera.main;
	}

	private void Update()
	{
		if (useGadget)
		{
			UseGadget();
		}
		else
		{
			effectArea.transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
			effectArea.transform.localScale = MathUtility.ConvertPhysicsScaleToTransformScale(ItemData.effectRange);
		}
	}

	protected abstract void UseGadget();

	private void OnDrawGizmos()
	{

		if (ItemData == null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, 3);
			return;
		}

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, ItemData.effectRange);
	}
}
