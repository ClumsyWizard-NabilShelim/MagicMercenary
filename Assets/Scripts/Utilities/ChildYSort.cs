using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildYSort : MonoBehaviour
{
	[SerializeField] private bool isBelowParent;
	[SerializeField] private SpriteRenderer parentRenderer;
    public SpriteRenderer ParentRenderer { private get; set; }
	[SerializeField] private SpriteRenderer spriteRenderer;

	private void Start()
	{
		if (parentRenderer != null)
			ParentRenderer = parentRenderer;
	}

	private void Update()
	{
		spriteRenderer.sortingOrder = ParentRenderer.sortingOrder + (isBelowParent ? -1 : 1);
	}
}
