using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float jumpForce = 10f; // Jump force
    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground check
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Jump();
        UpdateAnimation();
    }

    public float lastSpeed = 0f;

    public enum State
    {
        Walking, Idle
    }
    public State state;

    private void Move()
    {
        var newSpeed = Mathf.Sign(rb.velocity.x);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (state == State.Idle || newSpeed != lastSpeed)
            {
                state = State.Walking;
                anim.CrossFadeInFixedTime("PlayerLeft", 0f);
            }
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (state == State.Idle || newSpeed != lastSpeed)
            {
                state = State.Walking;
                anim.CrossFadeInFixedTime("PlayerRight", 0f);
            }
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            var h = rb.velocity.x;
            if (state != State.Idle)
            {
                state = State.Idle;
                string n = h < 0 ? "PlayerIdleLeft" : "PlayerIdleRight";
                anim.CrossFadeInFixedTime(n, 0f);
            }
            rb.velocity = new Vector2(rb.velocity.x * 0.6f, rb.velocity.y);
        }
        lastSpeed = newSpeed;
    }

    private void Jump()
    {
        // Perform a circle cast to check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
    }

    private void UpdateAnimation()
    {
        // Update grounded animation state
        anim.SetBool("isGrounded", isGrounded);

        // Update speed animation based on horizontal velocity when grounded
        if (isGrounded)
        {
            anim.SetFloat("Speed", rb.velocity.x);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the ground check area in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, groundCheckRadius);
    }
}
