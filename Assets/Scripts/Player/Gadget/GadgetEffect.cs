using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetEffect : MonoBehaviour
{
    private Vector2 targetSize;
	private bool grow;
	private bool destroy;

    public void Initialize(Vector2 size)
	{
		targetSize = size;
		transform.localScale = Vector2.zero;
		grow = true;
	}

	private void Update()
	{
		if (!grow)
			return;

		if (Mathf.Abs(transform.localScale.x - targetSize.x) <= 0.1f)
		{
			grow = false;

			if (destroy)
			{
				Destroy(gameObject);
				return;
			}

			StartCoroutine(Disappear());
		}
		else
		{
			transform.localScale = Vector3.Lerp(transform.localScale, targetSize, 5.0f * Time.deltaTime);
		}
	}

	private IEnumerator Disappear()
	{
		yield return new WaitForSeconds(2.0f);
		targetSize = Vector2.zero;
		grow = true;
		destroy = true;
	}
}
