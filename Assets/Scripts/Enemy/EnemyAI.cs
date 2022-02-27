using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
	None = -1,
	Patrol,
	Investigate,
	Attack,
}

public class EnemyAI : MonoBehaviour
{
	[SerializeField] private EnemyState state;
	private bool switchingState;
	private Rigidbody2D rb;
	private EnemyStats enemyStats;

	private EnemyModule currentModule;
	[SerializeField] private EnemyModule patrolling;
	[SerializeField] private EnemyModule suspicious;
	[SerializeField] private EnemyModule attacking;

	[SerializeField] private float detectionDelay;
	private float currentDetectionTime;
	private float elapsedTime;

	[Header("UI")]
	[SerializeField] private GameObject suspiciousIndicator;
	[SerializeField] private Image detectionBar;

	private void Start()
	{
		SetState(EnemyState.Patrol);
		ActivateSuspiciousIndicator(false);

		rb = GetComponent<Rigidbody2D>();
		enemyStats = GetComponentInChildren<EnemyStats>();
	}

	private void Update()
	{
		if (!switchingState)
		{
			currentModule.OnUpdate();
		}
		else
		{
			rb.velocity = Vector2.zero;
		}
	}

	public void TargetInView(float distanceMultiplier)
	{
		if(state == EnemyState.Patrol)
		{
			SetState(EnemyState.Investigate);
		}
		else if(state == EnemyState.Investigate)
		{
			if(currentDetectionTime <= 0)
			{
				SetState(EnemyState.Attack);
				ActivateSuspiciousIndicator(false);
			}
			else
			{
				elapsedTime += distanceMultiplier * Time.deltaTime;
				currentDetectionTime -= elapsedTime * Time.deltaTime;
				detectionBar.fillAmount = 1.0f - (currentDetectionTime / detectionDelay);
			}
		}
	}

	public void SetState(EnemyState state)
	{
		if (this.state == state)
			return;

		switchingState = true;

		this.state = state;

		ActivateSuspiciousIndicator(false);

		switch (state)
		{
			case EnemyState.Patrol:
				currentModule = patrolling;
				break;
			case EnemyState.Investigate:
				currentModule = suspicious;
				ActivateSuspiciousIndicator(true);
				break;
			case EnemyState.Attack:
				currentModule = attacking;
				break;
			default:
				break;
		}

		currentModule.OnInitialize();
		switchingState = false;
	}

	public EnemyState GetState()
	{
		return state;
	}

	private void ActivateSuspiciousIndicator(bool setActive)
	{
		suspiciousIndicator.SetActive(setActive);
		currentDetectionTime = detectionDelay;
		elapsedTime = 0;
		detectionBar.fillAmount = 0.0f;
	}

	public void HalfDetectionDelay()
	{
		detectionDelay = detectionDelay / 2.0f;
		currentDetectionTime = detectionDelay;
	}

	public void ZeroDetectionDelay()
	{
		detectionDelay = 0;
	}

	public EnemyStats GetEnemyStats()
	{
		return enemyStats;
	}
}