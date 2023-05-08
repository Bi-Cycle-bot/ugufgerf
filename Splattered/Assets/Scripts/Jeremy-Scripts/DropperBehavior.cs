using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperBehavior : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject dropObject;
    public enum Direction { Left, Right };
    #region Movement
    public float maxSpeed = 8;
    public Direction direction = Direction.Left;
    public Vector2 targetFollowRange = new Vector2(20, 20);
    public bool followTarget = false;
    private bool canMove;
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        lastDropTime = -10;
    }

    void FixedUpdate()
    {
        if(currentHealth <= 0) {
            if(dropOnDeath) {
                Instantiate(dropObject, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        canMove = Mathf.Abs(transform.position.x - target.transform.position.x) < targetFollowRange.x &&
                  Mathf.Abs(transform.position.y - target.transform.position.y) < targetFollowRange.y;
        if (followTarget && target.transform.position.x > transform.position.x)
        {
            direction = Direction.Right;
        }
        else if (followTarget && target.transform.position.x < transform.position.x)
        {
            direction = Direction.Left;
        }
        Drop();
        Move();

    }

    void Drop()
    {
        if (transform.position.x - target.transform.position.x < dropRange &&
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
        if (Mathf.Abs(transform.position.x - target.transform.position.x) < targetFollowRange.x && Mathf.Abs(transform.position.y - target.transform.position.y) < targetFollowRange.y)
            rb.velocity = (direction == Direction.Left) ? Vector2.left * targetSpeed : Vector2.right * targetSpeed;
        else
            rb.velocity = Vector2.zero;
    }

    void DamageDropper(float damage)
    {
        currentHealth -= damage;
    }
}