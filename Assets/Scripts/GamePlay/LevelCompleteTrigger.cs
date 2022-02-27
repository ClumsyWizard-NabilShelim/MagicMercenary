using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelCompleteTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			PlayerInside();
		}
	}

	protected abstract void PlayerInside();

	protected void LoadNextLevel()
	{
		SceneManagement.Instance.Load();
	}
}
