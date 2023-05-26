using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoldierMovement : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] private GameObject bulletPrefab;
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
    [SerializeField] private Vector2 groundCheckSize;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool isGrounded;

    private float lastAttackTime;

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
    }

    void FixedUpdate()
    {
        
        if (currentHealth <= 0)
        {
            deathParticles.transform.position = transform.position;
            deathParticles.Play();
            Destroy(gameObject);
        }
        canMove = Mathf.Abs(transform.position.x - target.transform.position.x) < Data.targetMaxCoordinates.x &&
                  Mathf.Abs(transform.position.y - target.transform.position.y) < Data.targetMaxCoordinates.y &&
                  Mathf.Abs(transform.position.x - target.transform.position.x) > Data.targetMinCoordinates.x &&
                  Mathf.Abs(transform.position.y - target.transform.position.y) > Data.targetMinCoordinates.y;
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;

        direction = (target.transform.position.x - transform.position.x > 0) ? 1 : -1;

        if (!isStunned)
        {
            Run();
        }

        if (Physics2D.OverlapBox(transform.position, hitbox.size, 0, LayerMask.GetMask("Player")))
        {
            playerMovement.damagePlayer(Data.attackDamage, Data.attackStunDuration, transform.position, Data.knockbackForce, false);
        }

        if (Time.time - lastAttackTime > Data.attackSpeed && canMove)
        {
            lastAttackTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }


    }

    void Update() {
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
        if(!canMove || Mathf.Abs(target.transform.position.x - transform.position.x) < 0.5f)
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

    public void damageSoldier(float damage, Vector2 bulletDirection, float knockbackforce)
    {
        // rb.AddForce((bulletDirection).normalized * knockbackforce, ForceMode2D.Impulse);
        // animator.SetTrigger("Stun");
        rb.AddForce(bulletDirection * knockbackforce, ForceMode2D.Impulse);
        StartCoroutine(Stun());
        currentHealth -= damage;
        // animator.SetTrigger("Unstun");
    }

    IEnumerator Stun()
    {
        isStunned = true;
        yield return new WaitForSeconds(Data.hitStunDuration);
        isStunned = false;
    }



}
