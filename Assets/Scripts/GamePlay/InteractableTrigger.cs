using System.Collections;
using UnityEngine;

public class InteractableTrigger : LevelCompleteTrigger
{
	[SerializeField] private GameObject interactionKeyMarker;
	private bool playerInside;

	private void Start()
	{
		interactionKeyMarker.SetActive(false);
	}

	protected override void PlayerInside()
	{
		playerInside = true;
		interactionKeyMarker.SetActive(true);
	}

	private void Update()
	{
		if(playerInside)
		{
			if (Input.GetKeyDown(KeyCode.E))
				LoadNextLevel();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			playerInside = false;
			interactionKeyMarker.SetActive(false);
		}
	}
}