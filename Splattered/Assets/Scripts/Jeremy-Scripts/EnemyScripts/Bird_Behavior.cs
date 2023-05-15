using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Behavior : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject target;
    Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    CircleCollider2D hitbox;
    private Bird_Data data;
    private float lastGunShot;
    private bool canMove;
    private float currentHealth;
    private float stunTime = 0;
    

    

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<CircleCollider2D>();
        data = GetComponent<Bird_Data>();
        currentHealth = data.maxHealth;
        target = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        canMove = Mathf.Abs(transform.position.x - target.transform.position.x) < data.targetFollowRange.x &&
                  Mathf.Abs(transform.position.y - target.transform.position.y) < data.targetFollowRange.y;
        data.targetLocation = (Vector2)target.transform.position + new Vector2(0, data.heightAboveTarget);
        if(Vector2.Distance(data.targetLocation, (Vector2)transform.position) > data.slowDistance)
            data.isUsingSlowSpeed = false;
        else if ((Vector2.Distance(data.targetLocation, (Vector2)transform.position) < data.slowDistance/1.5f))
            data.isUsingSlowSpeed = true;
        fly();

        if(Vector2.Distance(transform.position, target.transform.position) < data.attackRange && Time.time - lastGunShot > data.attackSpeed && canMove){
            lastGunShot = Time.time;
            GameObject bullet = Instantiate(data.bulletPrefab, transform.position, Quaternion.identity);
        }
        spriteRenderer.flipX = rb.velocity.x > 0;
    }

    void fly(){
        #region Acceleration Calculation
        float accelerationRate = data.accelAmount;
        float slowDistance = data.attackRange - data.heightAboveTarget;
        if (Vector2.Distance(data.targetLocation, (Vector2)transform.position) < slowDistance * 0.05f || !canMove){
            data.targetSpeed = 0;
            accelerationRate = data.deccelAmount;
        }

        else if(data.isUsingSlowSpeed == true){
            data.targetSpeed = data.maxSpeed * data.slowSpeedMultiplier;
            accelerationRate = data.deccelAmount;
        }
        else if (data.isUsingSlowSpeed == false)
            data.targetSpeed = data.maxSpeed;
        
        Vector2 direction = data.targetLocation  - (Vector2)transform.position;
        direction.Normalize();
        #endregion

        #region Apply Movement
        float speedDifference = data.targetSpeed - rb.velocity.magnitude;
        float movement = speedDifference * accelerationRate;
        rb.velocity = rb.velocity.magnitude * direction;
        rb.AddForce(movement * direction, ForceMode2D.Force);
        #endregion
        
    }

    public void DamageBird(float damage){
        currentHealth -= damage;
    }
    public void DamageBird(float damage, Vector2 knockback){
        currentHealth -= damage;
        rb.AddForce(knockback, ForceMode2D.Impulse);
    }
}
