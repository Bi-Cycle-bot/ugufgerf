using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D hitbox;
    public PlayerMovementData Data;

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
    private bool usingFasterGravity;
    public bool isJumping;
    public bool isSliding;
    public bool isFacingRight;
    public float slideDirection;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        Data = GetComponent<PlayerMovementData>();
        isSliding = false;
        isJumping = false;
        isFacingRight = true;
        Data.lastSlideTime = Time.time;
        Data.lastSlideTimeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        lastOnGround -= (lastOnGround > -0.1) ? Time.deltaTime : 0;
        moveInput.x = Input.GetAxisRaw("Horizontal");
        if(moveInput.x > 0)
        {
            isFacingRight = true;
        }
        else if(moveInput.x < 0)
        {
            isFacingRight = false;
        }
        moveInput.y = Input.GetAxisRaw("Vertical");
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground")))
            lastOnGround = Data.cyoteTime;
        if (lastOnGround > 0)
            isGrounded = true;
        else
            isGrounded = false;

        if (rb.velocity.y < 0.0f)
            isJumping = false;

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
        if(isSliding && Input.GetAxisRaw("Horizontal") == -slideDirection)
        {
            Debug.Log("Slide ended");
            isSliding = false;
            rb.velocity = new Vector2(Data.maxSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
        }
        else if(Time.time - Data.lastSlideTimeStart > Data.slideTime && isSliding)
        {
            Debug.Log("Slide ended");
            isSliding = false;
            rb.velocity = new Vector2(Data.maxSpeed * Input.GetAxisRaw("Horizontal"), rb.velocity.y);
        }
        #endregion

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            isSliding = false;
            Jump();
        }
        else if (Input.GetButtonDown("Slide") && Time.time - Data.lastSlideTime >= Data.slideCoolodown)
        {
            StartSlide();
            Debug.Log("started slide");
        }

    }

    void FixedUpdate()
    {
        if(!isSliding)
            Run();
        else    
            Slide();
        if(-rb.velocity.y > Data.maxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -Data.maxFallSpeed);
        
    }

    private void Run()
    {
        float targetSpeed = moveInput.x * Data.maxSpeed;

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
        // float targetSpeed = slideDirection * Data.slideSpeed;
        
        // #region Caluclate Acceleration
        // float accelRate = Data.runAccelAmount*Data.slideAccelMultiplier;
        // #endregion

        // #region Apply Movment
        // float speedDifference = targetSpeed - rb.velocity.x;
        // float movement = speedDifference * accelRate;
        // rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        // #endregion
        rb.velocity = new Vector2(slideDirection * Data.slideSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        lastOnGround = 0;

        float force = Data.jumpForce;
        // if (rb.velocity.y < 0)
        //     force -= rb.velocity.y;
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


}
