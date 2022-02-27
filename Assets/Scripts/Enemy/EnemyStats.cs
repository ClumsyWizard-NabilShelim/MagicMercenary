using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FearLevel
{
	None,
	Alert,
	Afraid,
	Berserk
}

public class EnemyStats : Damageable
{
	private Animator animator;
	private EnemyAI enemyAI;
	public FearLevel FearLevel { get; private set; }

	[SerializeField] private Transform fearLevelHolder;
	[SerializeField] private GameObject fearLevelMarkerPrefab;
	private List<GameObject> fearLevelMarkers = new List<GameObject>();
	[SerializeField] private GameObject deadEnemy;
	[SerializeField] private GameObject bloodEffect;

	private void Start()
	{
		enemyAI = GetComponentInParent<EnemyAI>();
		animator = GetComponentInParent<Animator>();

		for (int i = 0; i < Enum.GetValues(typeof(FearLevel)).Length; i++)
		{
			GameObject fearLevelMarker = Instantiate(fearLevelMarkerPrefab, fearLevelHolder);
			fearLevelMarker.SetActive(false);
			fearLevelMarkers.Add(fearLevelMarker);
		}
	}

	public override void Damage(int amount)
	{
		Instantiate(bloodEffect, transform.position, Quaternion.identity);
		animator.SetTrigger("Hurt");
		AudioManager.Instance.PlayAudio("Hit");
		CameraShake.Instance.ShakeObject(0.3f, ShakeMagnitude.Medium);
		currentHealth -= amount;

		if (currentHealth <= 0)
			DestroySelf();
	}

	public override void DestroySelf()
	{
		GameManager.Instance.EnemyKilled();
		CameraShake.Instance.ShakeObject(0.3f, ShakeMagnitude.Large);
		AudioManager.Instance.PlayAudio("Death");
		Instantiate(deadEnemy, transform.position, Quaternion.identity);
		Destroy(transform.parent.gameObject);
	}

	//Modifiers
	public void IncreaseFearLevel()
	{
		if (FearLevel == FearLevel.Berserk)
			return;

		FearLevel++;

		for (int i = 0; i < (int)FearLevel; i++)
		{
			fearLevelMarkers[i].SetActive(true);
		}

		if (FearLevel == FearLevel.Alert)
			enemyAI.HalfDetectionDelay();

		if (FearLevel == FearLevel.Berserk)
			enemyAI.ZeroDetectionDelay();
	}

	public void SetFearLevel(FearLevel fearLevel)
	{
		int diff = fearLevel - FearLevel;

		if (diff < 0)
		{
			Debug.Log("Cant decrease fear level");
			return;
		}

		for (int i = 0; i < diff; i++)
		{
			IncreaseFearLevel();
		}
	}
}
