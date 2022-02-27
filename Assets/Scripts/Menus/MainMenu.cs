using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public void Play()
	{
		SceneManagement.Instance.Load();
	}

	public void Credits()
	{
		SceneManagement.Instance.Load("Credits");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
