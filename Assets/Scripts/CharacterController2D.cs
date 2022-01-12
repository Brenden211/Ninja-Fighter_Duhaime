using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterController2D : MonoBehaviour
{
	AudioSource Jump;

	void Start()
	{
		Jump = GetComponent<AudioSource>();
	}

	[SerializeField] private float JumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;
	[SerializeField] private bool AirControl = false;
	[SerializeField] private LayerMask WhatIsGround;
	[SerializeField] private Transform GroundCheck;

	const float GroundedRadius = .2f;
	private bool Grounded;
	private Rigidbody2D Rigidbody2D;
	private bool FacingRight = true;
	private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
		Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		Grounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				Grounded = true;
		}
	}

	public void Move(float move, bool jump)
	{
		if (Grounded || AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, Rigidbody2D.velocity.y);
			Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref velocity, MovementSmoothing);

			if (move > 0 && !FacingRight)
			{
				Flip();
			}
			else if (move < 0 && FacingRight)
			{
				Flip();
			}

		}
		if (Grounded && jump)
		{
			Grounded = false;
			Rigidbody2D.AddForce(new Vector2(0f, JumpForce));
		}
	}


	private void Flip()
	{
		FacingRight = !FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}