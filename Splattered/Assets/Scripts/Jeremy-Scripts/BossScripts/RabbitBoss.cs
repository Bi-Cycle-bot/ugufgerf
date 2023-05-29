using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBoss : Boss
{
    [Header("GameObjects")]
    public GameObject bullet;
    [HideInInspector] public BunnyExplosion bunnyExplosion;
    private Rigidbody2D rb;
    private Collider2D hitbox;
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [Space(10)]
    [Header("Movement Settings")]
    #region GroundChecks
    public Transform groundCheck;
    public Vector2 groundCheckSize;

    public Transform wallCheckRight;
    public Transform wallCheckLeft;
    public Vector2 wallCheckSize;
    public bool isGrounded;
    public bool isTouchingWallRight;
    public bool isTouchingWallLeft;
    #endregion

    [Space(3)]
    public float maxSpeed = 8f;
    public float accelerationTime = 2f;
    [HideInInspector] public float runAccelAmount; //force applied to bunny to accelerate
    public float decelerationTime; //time it should take for bunny to decelerate from max speed to 0
    [HideInInspector] public float runDeccelAmount; //force applied to bunny to decelerate
    [HideInInspector] public float direction;
    [HideInInspector] public bool doNotRun;
    [HideInInspector] public bool choosingAttack;


    [Space(10)]
    [Header("Attack Settings")]
    public float timeBetweenAttacks = 0.4f;


    [Space(10)]
    [Header("JumpAttack")]
    public float timeBetweenJumps = 0.5f;
    [Range(0.01f, 10)] public float arialSpeedMultiplier;
    public float jumpAttackDirectDamage = 12f;
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
    public float jumpAttackSlamSpeed = 34f;
    [HideInInspector] public float jumpAttackGravityStrength;
    [HideInInspector] public float jumpAttackGravityScale;
    #endregion

    public bool isJumping;

    [Space(10)]
    [Header("DashAttack")]
    public float dashAttackDamage = 12f;
    public float dashAttackKnockback = 10f;
    public float dashAttackStunDuration = 0.7f;
    public float dashAttackSpeed = 38f;
    public float timeBeforeDashing = 0.6f;
    [HideInInspector] public bool isDashing;

    [Space(10)]
    [Header("Gun Attack")]
    public float gunAttackDamage = 5f;
    public float gunAttackKnockback = 5f;
    public float gunAttackStunDuration = 0.1f;
    public float gunAttackSpeed = 30f;
    public float mininumShots = 4f;
    public float maximumShots = 30f;
    [HideInInspector] public bool isShooting;

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
        currentHealth = maxHealth;
        doNotRun = false;
        bunnyExplosion = GetComponentInChildren<BunnyExplosion>();
        isDashing = false;
        isShooting = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = target.GetComponent<PlayerMovement>();
        // DashAttack.bulletPrefab = GameObject.Find
    }


    void FixedUpdate()
    {
        isTouchingWallRight = Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0, LayerMask.GetMask("Ground")) || Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0, LayerMask.GetMask("Wall")) ||
            Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0, LayerMask.GetMask("StickyWall"));
        isTouchingWallLeft = Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0, LayerMask.GetMask("Ground")) || Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0, LayerMask.GetMask("Wall")) ||
            Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0, LayerMask.GetMask("StickyWall"));
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground"));
        Run();
        bool isAttacking = isJumping || isDashing || isShooting || choosingAttack;
        if (!isAttacking && isGrounded)
            StartCoroutine(chooseAttack());
        if (!isAttacking && (isTouchingWallLeft && !isFacingRight() || isTouchingWallRight && isFacingRight()))
            ChangeDirection();

    }

    void Update()
    {
        spriteRenderer.flipX = direction < 0;
        if (currentHealth < 0)
            GameObject.Destroy(gameObject);
        if (Physics2D.OverlapBox(transform.position, hitbox.bounds.size, 0, LayerMask.GetMask("Player")))
            playerMovement.damagePlayer(dashAttackDamage, dashAttackStunDuration, rb.velocity, dashAttackKnockback, true);
    }


    public override void DamageBoss(float damageAmount)
    {
        currentHealth -= damageAmount;
    }

    private void Run()
    {
        // Debug.Log("Running");
        float targetSpeed = (isGrounded) ? direction * maxSpeed :
                                           direction * maxSpeed * arialSpeedMultiplier;
        if (doNotRun)
            targetSpeed = 0;

        #region Apply Movement
        float speedDifference = targetSpeed - rb.velocity.x;
        float movement = speedDifference * runAccelAmount;
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        #endregion

    }

    private IEnumerator JumpAttack()
    {
        isJumping = true;
        ChangeDirection();
        while (!isTouchingWallLeft && direction < 0 || !isTouchingWallRight && direction > 0)
        {
            if (isGrounded)
            {
                animator.SetTrigger("Idle");
                yield return new WaitForSeconds(timeBetweenJumps);
                animator.SetTrigger("Jump");
                Jump();
                StartCoroutine(StopRunning(timeBetweenJumps));
                yield return new WaitForSeconds(0.1f);
            }
            else if (rb.velocity.y < 0)
            {
                animator.SetTrigger("Fall");
                rb.velocity = new Vector2(0, -jumpAttackSlamSpeed);
                while (!isGrounded)
                {
                    yield return null;
                    rb.velocity = new Vector2(0, -jumpAttackSlamSpeed);
                }
                StartCoroutine(bunnyExplosion.Explode());
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return null;
            }
        }
        isJumping = false;
        ChangeDirection();
        doNotRun = false;
        animator.SetTrigger("Run");
    }
    private IEnumerator GunAttack()
    {
        ChangeDirection();
        int shotsToFire = (int)Random.Range(mininumShots, maximumShots);
        doNotRun = true;
        isShooting = true;
        for (int i = 0; i < shotsToFire; i++)
        {
            GameObject NewBullet =Instantiate(bullet, transform.position, Quaternion.identity);
            NewBullet.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        doNotRun = false;
        isShooting = false;
        ChangeDirection();
    }
    private IEnumerator DashAttack()
    {
        isDashing = true;
        doNotRun = true;
        float startTime = Time.time;
        animator.SetTrigger("Duck");
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(timeBeforeDashing);
        animator.SetTrigger("Run");
        Vector2 DashAttackDirection = target.transform.position - transform.position;
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        DashAttackDirection.Normalize();
        rb.gravityScale = 0;
        while (Vector2.Distance(transform.position, targetPosition) > 0.5f && Time.time - startTime < 3f)
        {
            DashAttackDirection = targetPosition - (Vector2)transform.position;
            DashAttackDirection.Normalize();
            rb.velocity = DashAttackDirection * dashAttackSpeed;
            yield return null;
        }
        rb.gravityScale = jumpAttackGravityScale;
        doNotRun = false;
        isDashing = false;
        ChangeDirection();
    }

    private IEnumerator chooseAttack()
    {
        choosingAttack = true;
        ChangeDirection();
        int attack = Random.Range(0, 3);
        yield return new WaitForSeconds(timeBetweenAttacks);
        switch (attack)
        {
            case 0:
                StartCoroutine(JumpAttack());
                break;
            case 1:
                StartCoroutine(DashAttack());
                break;
            case 2:
                StartCoroutine(GunAttack());
                Debug.Log("Gun Attack");
                break;
        }
        choosingAttack = false;
        ChangeDirection();
        animator.SetTrigger("Run");
    }

    private void Jump()
    {
        float force = jumpAttackJumpForce;
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        isGrounded = false;
    }

    public void ChangeDirection()
    {
        if (transform.position.x - target.transform.position.x > 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        // Debug.Log("Direction: " + direction);
    }

    private bool isFacingRight()
    {
        return rb.velocity.x >= 0;
    }

    IEnumerator StopRunning(float time)
    {
        doNotRun = false;
        yield return new WaitForSeconds(time);
        doNotRun = true;
    }

    void OnColliionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.damagePlayer(dashAttackDamage, dashAttackStunDuration, rb.velocity, dashAttackKnockback, true);
        }
    }
}
