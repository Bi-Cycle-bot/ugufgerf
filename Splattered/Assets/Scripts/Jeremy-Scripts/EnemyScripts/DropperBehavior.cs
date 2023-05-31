using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropperBehavior : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject dropObject;
    [SerializeField] public GameObject floatingDamage;
    private TextMeshPro textMesh;
    private Animator animator;
    public enum Direction { Left, Right };
    #region Movement
    public float maxSpeed = 8;
    public Direction direction = Direction.Left;
    public Vector2 targetMaxCoordinates = new Vector2(20, 20);
    public Vector2 targetMinCoordinates = new Vector2(-20, -20);
    public bool followTarget = false;
    private bool canMove;
    public bool isWalledLeft;
    public bool isWalledRight;
    
    #endregion

    #region Drop
    public float dropRange = 2;
    public float dropCooldown = 3;
    public bool destroyOnDrop = true;
    public bool dropOnDeath = false;
    private float lastDropTime;
    #endregion

    #region Health
    public float maxHealth = 50;
    [HideInInspector] public float currentHealth;
    #endregion
    private Rigidbody2D rb;
    private Collider2D hitbox;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        lastDropTime = Time.time-10;
        animator = GetComponent<Animator>();
        hitbox = GetComponent<Collider2D>();
        textMesh = floatingDamage.GetComponent<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(currentHealth <= 0) {
            if(dropOnDeath) {
                Instantiate(dropObject, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        if ((target.transform.position.x - transform.position.x) < targetMaxCoordinates.x &&
            (target.transform.position.y - transform.position.y) < targetMaxCoordinates.y &&
            (target.transform.position.x - transform.position.x) > targetMinCoordinates.x &&
            (target.transform.position.y - transform.position.y) > targetMinCoordinates.y &&
            (direction == Direction.Left && !isWalledLeft || direction == Direction.Right && !isWalledRight))
        {
            if(!canMove)
                animator.SetTrigger("StartRunning");
            canMove = true;
        }
        else
        {
            if(canMove)
                animator.SetTrigger("StopRunning");
            canMove = false;
        }
        if (followTarget && target.transform.position.x > transform.position.x)
        {
            direction = Direction.Right;
        }
        else if (followTarget && target.transform.position.x < transform.position.x)
        {
            direction = Direction.Left;
        }
        if(canMove)
            Move();

    }

    void Update()
    {
        Drop();
        spriteRenderer.flipX = direction == Direction.Right;
    }

    void Drop()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) < dropRange &&
            Time.time - lastDropTime > dropCooldown && canMove)
        {
            Instantiate(dropObject, transform.position, Quaternion.identity);
            if (destroyOnDrop)
            {
                Destroy(gameObject);
            }
            lastDropTime = Time.time;
        }
    }

    void Move()
    {
        float targetSpeed = Mathf.Abs(transform.position.x - target.transform.position.x) < 0.15f ? 0 : maxSpeed;
        if (canMove && !(hitbox.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
            hitbox.IsTouchingLayers(LayerMask.GetMask("Wall")) ||
            hitbox.IsTouchingLayers(LayerMask.GetMask("StickyWall"))))
            rb.velocity = (direction == Direction.Left) ? Vector2.left * targetSpeed : Vector2.right * targetSpeed;
        else
            rb.velocity = Vector2.zero;
    }

    public void DamageDropper(float damage)
    {
        currentHealth -= damage;
        textMesh.SetText(damage.ToString());
        GameObject newDamage = Instantiate(floatingDamage, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z) , transform.rotation);
        //Instantiate(floatingDamage, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z) , transform.rotation);
        //StartCoroutine(deleteFDamage());
        Destroy(newDamage, 3);
    }
}