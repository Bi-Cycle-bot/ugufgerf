using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoldierMovement : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] public GameObject floatingDamage;
    private TextMeshPro textMesh;
    private ParticleSystem deathParticles;
    GameObject deathParticlesInstance;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private Animator animator;
    Rigidbody2D rb;
    BoxCollider2D hitbox;
    private SoldierData Data;
    

    [HideInInspector] public Vector2 targetLocation;
    [HideInInspector] public float direction;
    [HideInInspector] public bool isUsingSlowSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private Vector2 groundCheckSize;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isWalledLeft;
    [HideInInspector] public bool isWalledRight;
    private float lastJumpTime;
    private float jumpCooldown = 0.04f;


    private float lastAttackTime;
    private float lastLandTime;

    [HideInInspector] public float currentHealth;
    private bool isStunned;
    private Color originalColor;
    public new Transform transform;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        Data = GetComponent<SoldierData>();
        animator = GetComponent<Animator>();
        isUsingSlowSpeed = false;
        playerMovement = target.GetComponent<PlayerMovement>();
        lastAttackTime = Time.time;
        currentHealth = Data.maxHealth;
        isStunned = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        deathParticles = GameObject.FindGameObjectWithTag("EnemyDeathParticles").GetComponent<ParticleSystem>();
        setJumpCooldown();
        rb.gravityScale = 3.5f;
        lastLandTime = Time.time;
        textMesh = floatingDamage.GetComponent<TextMeshPro>();
    }

    void FixedUpdate()
    {

        if (currentHealth <= 0)
        {
            deathParticles.transform.position = transform.position;
            deathParticles.Play();
            Destroy(gameObject);
        }
        canMove = (target.transform.position.x - transform.position.x) < Data.targetMaxCoordinates.x &&
                  (target.transform.position.y - transform.position.y) < Data.targetMaxCoordinates.y &&
                  (target.transform.position.x - transform.position.x) > Data.targetMinCoordinates.x &&
                  (target.transform.position.y - transform.position.y) > Data.targetMinCoordinates.y && lastLandTime + 0.5 < Time.time;
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground")))
        {
            if (!isGrounded)
            {
                animator.SetTrigger("Land");
                lastLandTime = Time.time;
                setJumpCooldown();
            }
            isGrounded = true;
        }
        else
            isGrounded = false;
        if (Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0, LayerMask.GetMask("Ground")) ||
            Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0, LayerMask.GetMask("Wall")) ||
            Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0, LayerMask.GetMask("StickyWall")))
        {
            isWalledLeft = true;
        }
        else
            isWalledLeft = false;
        if (Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0, LayerMask.GetMask("Ground")) ||
            Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0, LayerMask.GetMask("Wall")) ||
            Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0, LayerMask.GetMask("StickyWall")))
        {
            isWalledRight = true;
        }
        else
            isWalledRight = false;
        direction = (target.transform.position.x - transform.position.x > 0) ? 1 : -1;

        if (!isStunned)
        {
            Run();
            jump();
        }

        if (Physics2D.OverlapBox(transform.position, hitbox.size, 0, LayerMask.GetMask("Player")))
        {
            playerMovement.damagePlayer(Data.attackDamage, Data.attackStunDuration, transform.position, Data.knockbackForce, false);
        }

        if (Time.time - lastAttackTime > Data.attackSpeed && canMove)
        {
            lastAttackTime = Time.time;
            // GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }


    }

    void Update()
    {
        spriteRenderer.flipX = direction == 1 ? true : false;
    }

    void Run()
    {
        float targetSpeed = direction * Data.maxSpeed;
        if (!isGrounded)
            targetSpeed /= 1.9f;
        if (Vector2.Distance(target.transform.position, transform.position) < Data.attackRange * 2 / 3)
        {
            targetSpeed *= Data.slowSpeedMultiplier;
        }
        // if ( Mathf.Abs(target.transform.position.x - transform.position.x) < 0.5f)
        // {
        //     targetSpeed = 0.1f;
        //     Debug.Log("Soldier is close to player");
        // }
        if (!canMove || Mathf.Abs(target.transform.position.x - transform.position.x) < 0.5f)
        {
            animator.SetTrigger("Stun");
            targetSpeed = 0;
        }
        else
        {
            animator.SetTrigger("Unstun");
        }

        #region Acceleration Calculation
        float accelerationRate = Data.accelAmount;
        float slowDistance = Data.attackRange * 2 / 3;
        // targetSpeed = (Vector2.Distance(target.transform.position, transform.position) < slowDistance) ? targetSpeed * Data.slowSpeedMultiplier : targetSpeed;
        #endregion

        #region Apply Movement

        float speedDifference = targetSpeed - rb.velocity.x;
        float movement = speedDifference * accelerationRate;
        rb.AddForce(movement * Vector2.right, ForceMode2D.Impulse);
        #endregion

    }

    public void jump()
    {
        if (isGrounded && !isStunned && (isWalledRight && direction >= 0 || isWalledLeft && direction <= 0 || target.transform.position.y > transform.position.y + 1) &&
        Mathf.Abs(target.transform.position.x - transform.position.x) < Data.attackRange * 2 / 3 && Time.time - lastJumpTime > jumpCooldown && canMove)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
            setJumpCooldown();
            animator.SetTrigger("Jump");
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "OneWayGround" && target.transform.position.y < transform.position.y && Mathf.Abs(transform.position.x - target.transform.position.x) < 0.5f &&
            Time.time - lastJumpTime > jumpCooldown && canMove)
        {
            Physics2D.IgnoreCollision(other.collider, hitbox, true);
            StartCoroutine(addCollision(other));
            lastJumpTime = Time.time;
            setJumpCooldown();
        }
    }

    IEnumerator addCollision(Collision2D other)
    {
        yield return new WaitForSeconds(0.3f);
        Physics2D.IgnoreCollision(other.collider, hitbox, false);
    }

    public void damageSoldier(float damage, Vector2 bulletDirection, float knockbackforce)
    {
        // rb.AddForce((bulletDirection).normalized * knockbackforce, ForceMode2D.Impulse);
        // animator.SetTrigger("Stun");
        rb.AddForce(bulletDirection * knockbackforce, ForceMode2D.Impulse);
        StartCoroutine(Stun());
        currentHealth -= damage;
        textMesh.SetText(damage.ToString());
        Instantiate(floatingDamage, transform.position, Quaternion.identity);
        //StartCoroutine(deleteFDamage());
        Destroy(floatingDamage, 3);
        // animator.SetTrigger("Unstun");
    }

    IEnumerator Stun()
    {
        isStunned = true;
        yield return new WaitForSeconds(Data.hitStunDuration);
        isStunned = false;
    }

    IEnumerator deleteFDamage()
    {
        yield return new WaitForSeconds(3);
    }


    private void setJumpCooldown()
    {
        jumpCooldown = Random.Range(1.5f, 2f);
    }

}
