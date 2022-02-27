using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
	[SerializeField] protected int health;
	protected int currentHealth;

	protected void Awake()
	{
		currentHealth = health;
	}

	public abstract void Damage(int amount);
	public abstract void DestroySelf();
}