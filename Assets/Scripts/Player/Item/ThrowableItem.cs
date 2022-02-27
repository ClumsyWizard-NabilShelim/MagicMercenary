using System;
using System.Collections;
using UnityEngine;
using ClumsyWizard;

public class ThrowableItem : MonoBehaviour
{
	[SerializeField] private float placeableRange;
	protected Camera cam;

	public Action TargetReached { get; set; }
	private bool thrown;
	private Vector2 moveToPos;

	protected void Awake()
	{
		cam = Camera.main;
	}

	public bool Throw()
	{
		moveToPos = cam.ScreenToWorldPoint(Input.mousePosition);
		if (MathUtility.IsPointInsideOval(transform.position, placeableRange, moveToPos, true))
		{
			transform.SetParent(null);
			thrown = true;
			return true;
		}

		return false;
	}

	private void Update()
	{
		if (thrown)
		{
			if (Vector2.Distance(transform.position, moveToPos) <= 0.2f)
			{
				thrown = false;
				transform.position = moveToPos;
				TargetReached?.Invoke();
			}
			else
			{
				transform.position = Vector2.MoveTowards(transform.position, moveToPos, 100.0f * Time.deltaTime);
			}
		}
	}

	public float GetPlaceableRange()
	{
		return placeableRange;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, placeableRange);
		Gizmos.DrawWireSphere(transform.position, placeableRange * 0.6f);
	}
}