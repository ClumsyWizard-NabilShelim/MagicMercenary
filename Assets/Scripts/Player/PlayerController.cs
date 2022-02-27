using ClumsyWizard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator animator;
	private Camera cam;

	private Vector2 inputVector;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float crouchMoveSpeed;
	private bool crouching;
	[SerializeField] private float walkDistractArea;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		cam = Camera.main;
	}

	private void Update()
	{
		if (inputVector.x < 0)
			transform.rotation = Quaternion.Euler(0, 180, 0);
		else if (inputVector.x > 0)
			transform.rotation = Quaternion.Euler(0, 0, 0);

		PollInput();
	}

	private void PollInput()
	{
		inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		if (!crouching && rb.velocity != Vector2.zero)
			WalkDistract();

		if(Input.GetKeyDown(KeyCode.LeftShift))
			crouching = !crouching;

		animator.SetBool("Crouch", crouching);
		animator.SetBool("Walk", rb.velocity != Vector2.zero);
	}

	private void FixedUpdate()
	{
		rb.velocity = inputVector * (crouching ? crouchMoveSpeed : moveSpeed);
	}

	private void WalkDistract()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, walkDistractArea);

		if (cols != null)
		{
			for (int i = 0; i < cols.Length; i++)
			{
				IDistractable distractable = cols[i].GetComponent<IDistractable>();

				if (distractable != null)
				{
					distractable.Distract(transform.position);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, walkDistractArea);
	}
}
