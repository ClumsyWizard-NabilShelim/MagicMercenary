using System.Collections;
using UnityEngine;

public enum KeyColor
{
	Red,
	Goldern
}

public class LevelDoor : MonoBehaviour
{
	[SerializeField] private KeyColor openKeyColor;
	private Animator animator;
	private ItemManager itemManager;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private float checkRadius;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		Collider2D col = Physics2D.OverlapCircle(transform.position, checkRadius, playerLayer);

		if (col != null)
		{
			if (itemManager == null)
				itemManager = col.GetComponent<ItemManager>();

			if (itemManager.HasKey(openKeyColor))
				animator.SetTrigger("Open");
		}
	}

	public void OpenDone()
	{
		Destroy(this);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, checkRadius);
	}
}