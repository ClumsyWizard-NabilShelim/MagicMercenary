using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : StaticInstance<SceneManagement>
{
	private Animator animator;
	private bool isLoadingLevel;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void Load()
	{
		if (isLoadingLevel)
			return;

		StartCoroutine(LoadScene(""));
	}

	public void Load(string sceneName)
	{
		if (isLoadingLevel)
			return;

		StartCoroutine(LoadScene(sceneName));
	}

	private IEnumerator LoadScene(string sceneName)
	{
		isLoadingLevel = true;
		animator.SetTrigger("Open");
		yield return new WaitForSeconds(1.0f);
		isLoadingLevel = false;

		if(sceneName == "")
		{
			int i = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadSceneAsync(++i);
		}
		else
		{
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		}
	}
}