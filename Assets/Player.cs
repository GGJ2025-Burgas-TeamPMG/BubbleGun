using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Facing
    {
        Left,
        Right
    }
    public Facing facing = Facing.Right;

    public float moveSpeed = 5f; // Speed of movement
    public float jumpForce = 10f; // Jump force
    public float aerialControl = 0.1f;
    
    [SerializeField] private float defaultAerialControl = 0.02f;
    [SerializeField] private float defaultGravityScale = 4;
    [SerializeField] private float defaultMoveSpeed = 10;

    public PhysicsMaterial2D groundedMaterial, jumpingMaterial;

    private const float explosionStrengthDefault = 5f;
    private float explosionStrength = explosionStrengthDefault;
    public int explosionSize = 0;

    public double bubbleCooldown = 1;
    private double endTimer = 0;
    private double bubbleLifetime;
    public float bubbleProjSpeed = 5;
    public GameObject bubbleProjPrefab;

    [NonSerialized] public Rigidbody2D rb;
    private Animator anim;
    CapsuleCollider2D ownCapsule;
    CircleCollider2D gcCapsule;
    [SerializeField] private bool isGrounded;

    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground check
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection

    public void ResetControls(bool resetGravityScale = false, bool resetAerialControl = false, bool resetGroundControl = false)
    {
        if (resetGravityScale) rb.gravityScale = defaultGravityScale;
        if (resetAerialControl) this.aerialControl = defaultAerialControl;
        if(resetGroundControl) moveSpeed = defaultMoveSpeed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ownCapsule = GetComponent<CapsuleCollider2D>();
        gcCapsule = GetComponentInChildren<CircleCollider2D>();

        //bubbleProjPrefab = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Jump();
        UpdateAnimation();
        ExploadingBalloonPress();
        BubbleAttackPress();
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
            if (isGrounded) rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            else rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(-moveSpeed, rb.velocity.y), aerialControl);
            facing = Facing.Left;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (state == State.Idle || newSpeed != lastSpeed)
            {
                state = State.Walking;
                anim.CrossFadeInFixedTime("PlayerRight", 0f);
            }
            if (isGrounded) rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            else rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(moveSpeed, rb.velocity.y), aerialControl);
            facing = Facing.Right;
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
            if(isGrounded) rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y);
        }
        lastSpeed = newSpeed;
    }

    private void Jump()
    {
        // Perform a circle cast to check if the player is grounded
        bool isGrounded = false;

        Vector2 position = transform.GetChild(0).position + (Vector3) gcCapsule.offset;
        float radius = gcCapsule.radius;
        var hits = Physics2D.OverlapCircleAll(position, radius);

        foreach (var h in hits)
        {
            if (h == ownCapsule) continue;
            if(h == gcCapsule) continue;
            isGrounded = true;
        }
        this.isGrounded = isGrounded;
        rb.sharedMaterial = isGrounded ? groundedMaterial : jumpingMaterial;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }
    }

    private void UpdateAnimation()
    {
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

    private void ExploadingBalloonPress()
    {
        if (Input.GetKey(KeyCode.K))
        {
            explosionStrength += Time.deltaTime * 15;
            explosionSize = (int)Math.Ceiling(explosionStrength / 10);
        }

        else if (Input.GetKeyUp(KeyCode.K))
        {
            ExploadingBalloonDash(explosionSize);
        }

        if (explosionSize > 3)
        {
            ExploadingBalloonDash(explosionSize - 1);
        }
    }
    private void ExploadingBalloonDash(int strength)
    {
        strength = strength * 10 + 13;
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            direction = Vector2.down;
        }

        else
        {
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Right"))
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
        }

        float y = 0;

        if (direction.y == 0)
        {
            y = rb.velocity.y + 7;
        }

        rb.velocity = new Vector2(rb.velocity.x / 2 + direction.x * strength, direction.y * strength + y);
        explosionStrength = explosionStrengthDefault;
        explosionSize = 0;
    }
    
    GameObject spawnedBubbleProj;
    public void BubbleAttackPress()
    {

        //Debug.Log($"{Time.timeAsDouble} - {endTimer}");
        if (bubbleLifetime >= Time.timeAsDouble + 5)
        {
            Destroy(spawnedBubbleProj);
        }
        if (Input.GetKey(KeyCode.J) && Time.timeAsDouble >= endTimer)
        {
            bubbleLifetime = Time.timeAsDouble;
            if(facing == Facing.Right)
            {
                spawnedBubbleProj = (GameObject)Instantiate(bubbleProjPrefab, transform.position + Vector3.right, transform.rotation);
                spawnedBubbleProj.GetComponentInChildren<Rigidbody2D>().velocity = Vector3.right * bubbleProjSpeed;

            }
            else
            {
                spawnedBubbleProj = (GameObject)Instantiate(bubbleProjPrefab, transform.position + Vector3.left, transform.rotation);
                spawnedBubbleProj.GetComponentInChildren<Rigidbody2D>().velocity = Vector3.left * bubbleProjSpeed;
            }
            
            this.endTimer = Time.timeAsDouble + bubbleCooldown;
        }
        
    }
}
