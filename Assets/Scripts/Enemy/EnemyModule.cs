using UnityEngine;

public abstract class EnemyModule : MonoBehaviour
{
	protected EnemyMovement enemyMovement;

	protected virtual void Awake()
	{
		enemyMovement = GetComponent<EnemyMovement>();
	}

	public abstract void OnInitialize();
	protected abstract void OnEndReached();
	public abstract void OnUpdate();
}