using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPad : MonoBehaviour
{
    public float springForce = 30f;
    public Vector2 manualDirection = new Vector2(0, 1);
    public float horizontalForceMultiplier = 3f;
    public float stunDuration = 0.1f;
    public PlayerMovement playerMovement;
    public bool useManualDirection = false;
    

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.isSliding = false;
            Vector2 direction = Vector2.one;
            if (useManualDirection)
            {
                direction = manualDirection.normalized;
            }
            else
            {
                direction = new Vector2(transform.up.x * horizontalForceMultiplier, transform.up.y);
            }
            playerMovement.directionalKnockback((Vector2)transform.up, springForce, horizontalForceMultiplier);
            playerMovement.stunDuration = stunDuration;
            playerMovement.isStunned = true;
            Debug.Log(transform.up);
        }
    }
}
