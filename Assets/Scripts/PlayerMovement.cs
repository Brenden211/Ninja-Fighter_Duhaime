using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float checkRadius;
    public int maxJumpCount;
    public Transform groundCheck;
    public LayerMask groundObjects;
    public Animator animator;
    public bool isGrounded;
    public int jumpCount;
    public Transform AttackCheck;
    public float AttackRange = 0.5f;
    public LayerMask enemy;
    public int AttackDamage = 20;
    public float AttackRate = 2;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDirection;
    private bool isJumping = false;
    private float NextAttackTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Start()
    {
        jumpCount = maxJumpCount;

        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);

        moveSpeed = 4;
    }

    void Update()
    {
        if (Time.time >= NextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && (isGrounded))
            {
                Attack();
                NextAttackTime = Time.time + 1f / AttackRate;
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(moveDirection));

        ProcessInputs();

        Animate();

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded)
        {
            jumpCount = maxJumpCount;
            animator.SetBool("Jump", false);
        }

        if (!isGrounded)
        {
            animator.SetBool("Jump", true);
        }

        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (isJumping)
        {
            animator.SetBool("Jump", true);
            rb.AddForce(new Vector2(0f, jumpForce));
            jumpCount--;
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

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            isJumping = true;
            jumpCount--;
        }
    }

    private void FlipCharacter()
    {
		facingRight = !facingRight;
		transform.Rotate(0f, 180f, 0f);
    }

    void Attack()
    {
        moveSpeed = 0;
        animator.SetTrigger("Attack");
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackCheck.position, AttackRange, enemy);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Health>().TakeDamage(AttackDamage);
        }

        moveSpeed = 4;
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene("GameLost");
    }
}


