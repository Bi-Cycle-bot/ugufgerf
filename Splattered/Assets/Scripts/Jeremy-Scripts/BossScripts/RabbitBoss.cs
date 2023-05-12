using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBoss : Boss
{
    [Header("GameObjects")]
    public GameObject bullet;
    public GameObject jumpAttackExplosion;
    private Rigidbody2D rb;
    private Collider2D hitbox;
    [Space(10)]
    [Header("Movement Settings")]
    #region GroundChecks
    public Transform groundCheck;
    public Vector2 groundCheckSize;

    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public Vector2 wallCheckSize;
    private bool isGrounded;
    private bool isTouchingWallRight;
    private bool isTouchingWallLeft;
    #endregion

    [Space(3)]
    public float maxSpeed = 11f;
    public float accelerationTime = 2f;
    [HideInInspector] public float runAccelAmount; //force applied to bunny to accelerate
    public float decelerationTime; //time it should take for bunny to decelerate from max speed to 0
    [HideInInspector] public float runDeccelAmount; //force applied to bunny to decelerate
    [HideInInspector] public Vector2 direction;
 

    


    [Space(10)]
    [Header("JumpAttack")]
    public float timeBetweenJumps = 0.5f;
    [Range(0.01f, 1)]public float arialSpeedMultiplier;
    public float jumpAttackDirectDamage = 10f;
    public float jumpAttackDirectKnockback = 10f;
    public float jumpAttackDirectStunDuration = 0.7f;
    [Space(3)]
    public float jumpAttackExplosionDamage = 6f;
    public float jumpAttackExplosionKnockback = 5f;
    public float jumpAttackExplosionStunDuration = 0.1f;
    [Space(3)]
    public float jumpAttackHeight = 20f;
    public float jumpAttackToApexTime = 1f;
    [Space(3)]

    [HideInInspector] public float jumpAttackJumpForce;
    #region JumpAttackGravity
    public float jumpAttackSlamSpeed = 30f;
    [HideInInspector] public float jumpAttackGravityStrength;
    [HideInInspector] public float jumpAttackGravityScale;
    #endregion

    [HideInInspector] public bool isJumping;

    [Space(10)]
    [Header("DashAttack")]
    public float dashAttackDamage = 8f;
    public float dashAttackSpeed = 30f;
    [HideInInspector] public Vector2 dashAttackDirection;

    private void OnValidate()
    {
        #region Run Calculations
        // Calculate acceleration and deceleration amounts
        runAccelAmount = (50f * accelerationTime) / maxSpeed;
        runDeccelAmount = maxSpeed / decelerationTime;
        #endregion

        #region Gravity Calculations
        // Calculate gravity strength and scale
        jumpAttackGravityStrength = -(2 * jumpAttackHeight) / Mathf.Pow(jumpAttackToApexTime, 2);
        jumpAttackGravityScale = jumpAttackGravityStrength / Physics2D.gravity.y;
        #endregion

        #region Jump Calculations
        jumpAttackJumpForce = Mathf.Abs(jumpAttackGravityStrength) * jumpAttackToApexTime;
        #endregion
    }


    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        target = GameObject.FindGameObjectWithTag("Player");
        isJumping = false;
        ChangeDirection();
        rb.gravityScale = jumpAttackGravityScale;
    }

    void fixedUpdate()
    {
        isTouchingWallRight = Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0, LayerMask.GetMask("Ground"));
        isTouchingWallLeft = Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0, LayerMask.GetMask("Ground"));
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground"));
    }

    public override void DamageBoss(float damageAmount)
    {
        currentHealth -= damageAmount;
    }

    private void Run()
    {
        float targetSpeed = (isGrounded) ? direction.x * maxSpeed :
                                           direction.x * maxSpeed * arialSpeedMultiplier;

        #region Apply Movement
        float speedDifference = targetSpeed - rb.velocity.x;
        float movement = speedDifference * runAccelAmount;
        rb.AddForce(movement * Vector2.right, ForceMode2D.Impulse);
        #endregion

    }

    private IEnumerator JumpAttack()
    {
        isJumping = true;
        ChangeDirection();
        while (!isTouchingWallLeft && !isFacingRight() || !isTouchingWallRight && isFacingRight())
        {
            Run();
            if (isGrounded)
            {
                yield return new WaitForSeconds(timeBetweenJumps);
                Jump();
            }
            else if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(0, -jumpAttackSlamSpeed);
                while (!isGrounded && rb.velocity.y < -0.1f)
                {
                    yield return null;
                    rb.velocity = new Vector2(0, -jumpAttackSlamSpeed);
                }
                Instantiate(jumpAttackExplosion, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                isJumping = false;
            }
        }
    }

    private void Jump()
    {
        float force = jumpAttackJumpForce;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    public void ChangeDirection()
    {
        if (transform.position.x - target.transform.position.x > 0)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
    }

    private bool isFacingRight()
    {
        return direction.x > 0;
    }
}
