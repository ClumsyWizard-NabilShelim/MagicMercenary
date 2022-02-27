using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : Damageable
{
	private Animator animator;
    [SerializeField] private int mana;
    private int currentMana;

	[SerializeField] private Transform healthHolder;
	[SerializeField] private Transform manaHolder;

	[SerializeField] private GameObject healthPrefab;
	[SerializeField] private GameObject manaPrefab;
	[SerializeField] private GameObject bloodEffect;

	private void Start()
	{
		currentMana = mana;
		animator = GetComponentInParent<Animator>();
		UpdateHealth();
		UpdateMana();

		GameManager.Instance.AddToOnEnemyKilled(() =>
		{
			AlterMana(1);
		});
	}

	public override void Damage(int amount)
	{
		Instantiate(bloodEffect, transform.position, Quaternion.identity);
		animator.SetTrigger("Hurt");
		AudioManager.Instance.PlayAudio("Hit");
		CameraShake.Instance.ShakeObject(0.3f, ShakeMagnitude.Medium);
		currentHealth -= amount;

		UpdateHealth();

		if (currentHealth <= 0)
			DestroySelf();
	}

	private void UpdateHealth()
	{
		for (int i = 0; i < healthHolder.childCount; i++)
		{
			Destroy(healthHolder.GetChild(i).gameObject);
		}

		for (int i = 0; i < currentHealth; i++)
		{
			Instantiate(healthPrefab, healthHolder);
		}
	}

	private void UpdateMana()
	{
		for (int i = 0; i < manaHolder.childCount; i++)
		{
			Destroy(manaHolder.GetChild(i).gameObject);
		}

		for (int i = 0; i < currentMana; i++)
		{
			Instantiate(manaPrefab, manaHolder);
		}
	}

	public override void DestroySelf()
	{
		CameraShake.Instance.ShakeObject(0.3f, ShakeMagnitude.Large);
		AudioManager.Instance.PlayAudio("Death");
		GameManager.Instance.LevelFailed();
		Destroy(transform.parent.gameObject);
	}

	//Modifiers
	public void AlterMana(int amount)
	{
		if (currentMana + amount > mana)
			return;

		currentMana += amount;
		UpdateMana();
	}

	public bool HasEnoughMana(int amount)
	{
		return currentMana >= amount;
	}
}
