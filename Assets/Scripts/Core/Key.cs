using System.Collections;
using UnityEngine;

public class Key : MonoBehaviour
{
	[SerializeField] private KeyColor keyColor;
	[SerializeField] private GameObject keyPopEffect;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private float checkRadius;

	private void Update()
	{
		Collider2D col = Physics2D.OverlapCircle(transform.position, checkRadius, playerLayer);

		if(col != null)
		{
			col.GetComponent<ItemManager>().AddKey(keyColor);
			Instantiate(keyPopEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, checkRadius);
	}
}