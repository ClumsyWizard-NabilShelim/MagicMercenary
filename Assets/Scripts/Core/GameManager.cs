using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private Animator animator;
	[SerializeField] private GameObject passedPanel;
	[SerializeField] private GameObject failedPanel;
	private Action OnEnemyKilled;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}
	
	public void EnemyKilled()
	{
		OnEnemyKilled?.Invoke();
	}

	public void AddToOnEnemyKilled(Action action)
	{
		OnEnemyKilled += action;
	}
	//UI
	public void LevelPassed()
	{
		passedPanel.SetActive(true);
		animator.SetTrigger("ShowPanel");
	}

	public void LevelFailed()
	{
		failedPanel.SetActive(true);
		animator.SetTrigger("ShowPanel");
	}

	public void Next()
	{
		if (SceneManager.GetActiveScene().name == "Leve_3")
			SceneManagement.Instance.Load("Credits");
		else
			SceneManagement.Instance.Load();
	}

	public void Retry()
	{
		SceneManagement.Instance.Load(SceneManager.GetActiveScene().name);
	}

	public void MainMenu()
	{
		SceneManagement.Instance.Load("MainMenu");
	}
}
