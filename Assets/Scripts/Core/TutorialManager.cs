using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	private void Start()
	{
		Time.timeScale = 0.0f;
	}

	public void Close()
	{
		Time.timeScale = 1.0f;
		Destroy(gameObject);
	}
}
