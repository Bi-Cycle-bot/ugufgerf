using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    UnityEventQueueSystem eventQueue;
    Rigidbody2D rb;
    BoxCollider2D hitbox;
    SpawnPoint spawnPoint;
    public PlayerMovementData Data;
    private SpriteRenderer spriteRenderer;
    private Oscillation[] oscillations;
    private Camera cam;
    private CameraManager cameraManager;
    private Animator animator;

    #region INPUT PARAMETERS
    private Vector2 moveInput;
    #endregion

    #region CheckGround
    //set in inspector
    [Header("ground check")]
    [SerializeField] private Transform groundCheck; //position of ground check
    [SerializeField] private Vector2 groundCheckSize; //radius of ground check
    public float lastOnGround; //last time player was on ground
    [SerializeField] private bool isGrounded; //is player on ground
    #endregion

    #region State
    [HideInInspector] private bool usingFasterGravity;
    private bool isJumping;
    [HideInInspector] public bool isSliding;
    [HideInInspector] private bool wasSliding;
    [HideInInspector] public bool isFacingRight;
    [HideInInspector] public float slideDirection;
    [HideInInspector] public bool isStunned;
    [HideInInspector] public bool isInvincible;
    [HideInInspector] private bool isFlashing;
    private bool isFalling;

    #endregion

    #region Health and Damage
    [HideInInspector] public float health;
    private float maxHealth;
    public float stunDuration;
    private float invincibilityDuration;
    #endregion

    #region Color
    private Color originalColor;
    #endregion

    #region Events
    public delegate void deathEvent();
    public static event deathEvent onDeath;
    #endregion

    #region CheckGround
    //set in inspector
    [Header("wall check")]
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector2 wallJumpingPower;

    private bool isWallSliding;
    private bool isWalled;
    private float lastOnWall;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.01f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.01f;
    private float wallSlidingSpeed = 2.0f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        Data = GetComponent<PlayerMovementData>();
        spawnPoint = GetComponent<SpawnPoint>();
        isSliding = false;
        isJumping = false;
        isFacingRight = true;
        isFalling = false;
        Data.lastSlideTime = Time.time - 4f;
        Data.lastSlideTimeStart = Time.time - 4f;
        health = Data.maxHealth;
        maxHealth = Data.maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        oscillations = gameObject.GetComponents<Oscillation>();
        cam = Camera.main;
        cameraManager = cam.GetComponent<CameraManager>();
        cameraManager.addPosOscillation(oscillations[0], oscillations[1]);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
        #region State Calculations
        if (stunDuration > 0)
        {
            stunDuration -= Time.deltaTime;
            isStunned = true;
            invincibilityDuration = Data.invincibilityTime;
        }
        else
        {
            isStunned = false;
        }

        if (invincibilityDuration > 0)
        {
            invincibilityDuration -= Time.deltaTime;
        }

        isInvincible = (invincibilityDuration > 0 || isStunned || isSliding) ? true : false;
        lastOnGround -= (lastOnGround > -0.1) ? Time.deltaTime : 0;

        moveInput.x = Input.GetAxisRaw("Horizontal");
        if (moveInput.x > 0)
        {
            isFacingRight = true;
        }
        else if (moveInput.x < 0)
        {
            isFacingRight = false;
        }
        moveInput.y = Input.GetAxisRaw("Vertical");
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground")))
            lastOnGround = Data.coyoteTime;
        if (lastOnGround > 0)
        {
            if (isGrounded == false)
                setAnimationTo("PlayerLanding");
            isGrounded = true;
        }
        else
            isGrounded = false;
        // Debug.Log(lastOnGround);
        if (rb.velocity.y < -0.01f /*&& !isWalled*/)
        {
            if(!isFalling)
            {
                setAnimationTo("PlayerFall");
            }
            isFalling = true;
            isJumping = false;
        }
        else
        {
            if(isFalling && !isSliding && !isJumping)
            {
                setAnimationTo("PlayerWalk");
            }
            isFalling = false;
        }
        #endregion

        #region Gravity
        if ((rb.velocity.y < 0 || !Input.GetButton("Jump")) || !isJumping)
            usingFasterGravity = true;
        else
            usingFasterGravity = false;

        if (usingFasterGravity)
            setGravityScale(Data.gravityScale * Data.fallGravityMultiplier);
        else
            setGravityScale(Data.gravityScale);
        #endregion

        #region Slide Stop
        if (isSliding && Input.GetAxisRaw("Horizontal") == -slideDirection)
        {
            setAnimationTo("PlayerWalk");
            isSliding = false;
            rb.velocity = new Vector2(Data.maxSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
        }
        else if (Time.time - Data.lastSlideTimeStart > Data.slideTime && isSliding)
        {
            setAnimationTo("PlayerWalk");
            isSliding = false;
            rb.velocity = new Vector2(Data.maxSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
        }
        #endregion


        #region Input

        if (Input.GetButtonDown("Jump") && isGrounded && !isStunned)
        {
            isJumping = true;
            isSliding = false;
            Jump();
        }
        else if (Input.GetButtonDown("Slide") && Time.time - Data.lastSlideTime >= Data.slideCoolodown && !isSliding && !isStunned)
        {
            StartSlide();
            // Debug.Log("started slide");
        }

        #endregion

        if (rb.velocity.y < -Data.maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -Data.maxFallSpeed);
        }

        if (health <= 0)
        {
            spawnPoint.respawnAtCheckpoint();
            health = maxHealth;
        }
        if(!isGrounded)
        {
            IsWalled();
            WallSlide();
            WallJump();
        }

        #region Animations
        if (wasSliding && Time.time - Data.lastSlideTime >= Data.slideCoolodown)
        {
            StartCoroutine(canSlideAnimation());
        }
        wasSliding = Time.time - Data.lastSlideTime < Data.slideCoolodown;

        if(isSliding)
            setAnimationTo("PlayerSlide");
        else if (rb.velocity.y > 0.01f && !isWalled && !isJumping)
            setAnimationTo("PlayerJump");
        #endregion
        spriteRenderer.flipX = !isFacingRight;
        // if(isGrounded && !animator.GetCurrentAnimatorStateInfo(3).IsName("PlayerWalk") && !animator.GetCurrentAnimatorStateInfo(3).IsName("PlayerLanding") && !isJumping && !isSliding && !isFalling)
        // {
        //     setAnimationTo("PlayerWalk");
        // }
    }

    void FixedUpdate()
    {
        if (!isSliding)
            Run();
        else
            Slide();
        if (-rb.velocity.y > Data.maxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -Data.maxFallSpeed);

    }

    private void Run()
    {
        float targetSpeed = (!isStunned) ? moveInput.x * Data.maxSpeed : 0f;

        #region Caluclate Acceleration
        float accelRate;
        if (isGrounded)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        #endregion

        #region Apply Movment
        float speedDifference = targetSpeed - rb.velocity.x;
        float movement = speedDifference * accelRate;
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        // Debug.Log("force: " + movement * Vector2.right);
        #endregion
    }
    private void StartSlide()
    {
        Data.lastSlideTimeStart = Time.time;
        isJumping = false;
        isSliding = true;
        slideDirection = (isFacingRight) ? 1f : -1f;
        rb.velocity = Vector2.zero;
    }
    private void Slide()
    {
        Data.lastSlideTime = Time.time;
        if (Data.slideStopsYMovement)
            rb.velocity = new Vector2(slideDirection * Data.slideSpeed, 0);
        else
            rb.velocity = new Vector2(slideDirection * Data.slideSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        lastOnGround = 0;
        animator.Play("PlayerJumpUp");
        float force = Data.jumpForce;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(force * Vector2.up, ForceMode2D.Impulse);
    }
    private void setGravityScale(float newGravityScale)
    {
        rb.gravityScale = newGravityScale;
    }

    public void knockbackFromPosition(Vector2 position, float force)
    {
        rb.velocity = Vector2.zero;
        isJumping = false;
        Vector2 direction = (Vector2)transform.position - position;
        direction.Normalize();
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void directionalKnockback(Vector2 direction, float force)
    {
        rb.velocity = Vector2.zero;
        isJumping = false;
        direction.Normalize();
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void directionalKnockback(Vector2 direction, float force, float horizontalForceMultiplier)
    {
        rb.velocity = Vector2.zero;
        isJumping = false;
        direction.Normalize();
        direction.x *= horizontalForceMultiplier;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public bool damagePlayer(float damage, float stunDuration, Vector2 knockbackDirectionOrPosition, float knockbackForce, bool usingDirectionalKnockback)
    {
        if (!isInvincible)
        {
            health -= damage;
            if (usingDirectionalKnockback)
                directionalKnockback(knockbackDirectionOrPosition, knockbackForce);
            else
                knockbackFromPosition(knockbackDirectionOrPosition, knockbackForce);
            this.stunDuration = stunDuration;
            invincibilityDuration = Data.invincibilityTime;
            spriteRenderer.color -= new Color(0, 0, 0, 0);
            // Debug.Log("Player health = " + health);
            isInvincible = true;
            isStunned = true;
            StartCoroutine(hitAnimation());
            oscillations[0].reset();
            oscillations[1].reset();
            return true;
        }
        return false;
    }

    public void increaseMaxHealth(int amount)
    {
        maxHealth += amount;
        health += amount;
    }

    public void heal(int amount)
    {
        if ((health + amount) <= maxHealth)
        {
            health += amount;
        }
        else if ((health + amount) > maxHealth)
        {
            health = maxHealth;
        }
    }

    IEnumerator hitAnimation()
    {
        // float startTime = Time.time;
        bool isInvisible = false;
        while (isStunned || invincibilityDuration > 0)
        {
            spriteRenderer.color = (isInvisible) ? originalColor : Color.clear;
            isInvisible = !isInvisible;
            if (isInvisible)
                yield return new WaitForSeconds(Data.hitFlashTime / 2);
            else
                yield return new WaitForSeconds(Data.hitFlashTime);
        }
        spriteRenderer.color = originalColor;
    }

    IEnumerator canSlideAnimation()
    {
        spriteRenderer.color = new Color(0.35f, 0.35f, 0.9f, 1);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    private void IsWalled()
    {
        lastOnWall -= (lastOnWall > -0.1) ? Time.deltaTime : 0;

        if (Physics2D.OverlapCircle(wallCheckLeft.position, 0.2f, wallLayer))
        {
            lastOnWall = 0.1f;
            wallJumpingDirection = transform.localScale.x;
        }
        else if (Physics2D.OverlapCircle(wallCheckRight.position, 0.2f, wallLayer))
        {
            lastOnWall = 0.1f;
            wallJumpingDirection = -transform.localScale.x;
        }

        if (lastOnWall > 0)
        {
            if (isWalled == false)
                setAnimationTo("PlayerIdle");
            isWalled = true;
            isFalling = false;
        }
        else
        {
            isWalled = false;
        }
    }

    private void WallSlide()
    {
        if (isWalled && !isGrounded && lastOnWall >= 0.1f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding || lastOnWall >= 0.0f)
        {
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0.0f)
        {
            lastOnWall = 0;
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0.0f;
            setAnimationTo("PlayerJumpUp");

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    public void setAnimationTo(string animationName)
    {
        animator.Play(animationName);
    }

}
