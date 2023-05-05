using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoldierMovement : MonoBehaviour
{
    [SerializeField] public GameObject target;
    Rigidbody2D rb;
    BoxCollider2D hitbox;
    private SoldierData Data;

    [HideInInspector] public Vector2 targetLocation;
    [HideInInspector] public float direction;
    [HideInInspector] public bool isUsingSlowSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<BoxCollider2D>();
        Data = GetComponent<SoldierData>();
        isUsingSlowSpeed = false;

        

    }

    void FixedUpdate()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
            isGrounded = false;

        direction = (target.transform.position.x - transform.position.x > 0) ? 1 : -1;

        if(isGrounded)
        {
            Run();
        }


    }

    void Run()
    {
        float targetSpeed = direction * Data.maxSpeed;

        #region Acceleration Calculation
        float accelerationRate = Data.accelAmount;
        float slowDistance = Data.attackRange;
        #endregion

        #region Apply Movement

        float speedDifference = targetSpeed - rb.velocity.x;
        float movement = speedDifference * accelerationRate;
        rb.AddForce(movement * Vector2.right, ForceMode2D.Impulse);
        #endregion

    }
}