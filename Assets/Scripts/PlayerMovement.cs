using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed;
    public float jumpForce;
    public Transform cellingCheck;

    private Rigidbody2D rb;
	private bool facingRight = true;
	private float moveDirection;
    private bool isJumping = false;

    private void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        ProcessInputs();

        Animate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }

        isJumping = false;
    }

    private void Animate()
    {
        if (moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    private void ProcessInputs()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    private void FlipCharacter()
    {
		facingRight = !facingRight;
		transform.Rotate(0f, 180f, 0f);
    }
}


