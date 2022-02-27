using System.Collections;
using UnityEngine;

public class VolumeTrigger : LevelCompleteTrigger
{
	protected override void PlayerInside()
	{
		LoadNextLevel();
	}
}