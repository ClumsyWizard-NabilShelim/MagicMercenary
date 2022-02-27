using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YSort : MonoBehaviour
{
	[SerializeField] private bool runOnce;
    [SerializeField] private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		if(runOnce)
		{
			Sort();
			Destroy(this);
		}
	}

	private void Sort()
	{
		float basePos = spriteRenderer.bounds.min.y;
		spriteRenderer.sortingOrder = (int)(basePos * -100);
	}

	private void Update()
	{
		Sort();
	}
}
