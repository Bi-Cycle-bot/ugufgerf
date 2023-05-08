using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoldierMovement : MonoBehaviour
{
    [SerializeField] public GameObject target;
    [SerializeField] private GameObject bulletPrefab;
    private PlayerMovement playerMovement;
    Rigidbody2D rb;
    BoxCollider2D hitbox;
    private SoldierData Data;

    [HideInInspector] public Vector2 targetLocation;
    [HideInInspector] public float direction;
    [HideInInspector] public bool isUsingSlowSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [HideInInspector] public bool canMove;
    public bool isGrounded;

    private float lastAttackTime;

    public float currentHealth;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        Data = GetComponent<SoldierData>();
        isUsingSlowSpeed = false;
        playerMovement = target.GetComponent<PlayerMovement>();
        lastAttackTime = Time.time;
        currentHealth = Data.maxHealth;

    }

    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        canMove = Mathf.Abs(transform.position.x - target.transform.position.x) < Data.targetFollowRange.x &&
                  Mathf.Abs(transform.position.y - target.transform.position.y) < Data.targetFollowRange.y;
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;

        direction = (target.transform.position.x - transform.position.x > 0) ? 1 : -1;

        if (isGrounded)
        {
            Run();
        }

        if (Physics2D.OverlapBox(transform.position, hitbox.size, 0, LayerMask.GetMask("Player")))
        {
            playerMovement.damagePlayer(Data.attackDamage, Data.stunDuration, transform.position, Data.knockbackForce, false);
        }

        if (Time.time - lastAttackTime > Data.attackSpeed && canMove)
        {
            lastAttackTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }


    }

    void Run()
    {
        float targetSpeed = direction * Data.maxSpeed;
        if (Vector2.Distance(target.transform.position, transform.position) < Data.attackRange * 2 / 3)
        {
            targetSpeed *= Data.slowSpeedMultiplier;
        }
        if (Vector2.Distance(target.transform.position, transform.position) < 0.05f || !canMove)
        {
            targetSpeed = 0;
        }

        #region Acceleration Calculation
        float accelerationRate = Data.accelAmount;
        float slowDistance = Data.attackRange * 2 / 3;
        targetSpeed = (Vector2.Distance(target.transform.position, transform.position) < slowDistance) ? targetSpeed * Data.slowSpeedMultiplier : targetSpeed;
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
        rb.AddForce(transform.right * knockbackforce, ForceMode2D.Impulse);
        currentHealth -= damage;
    }


}
