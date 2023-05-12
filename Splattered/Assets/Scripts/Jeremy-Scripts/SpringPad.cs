using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPad : MonoBehaviour
{
    public float springForce = 30f;
    public float horizontalForceMultiplier = 3f;
    public float stunDuration = 0.1f;
    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 direction = new Vector2(transform.up.x * horizontalForceMultiplier, transform.up.y);
            playerMovement.directionalKnockbackWithHorizontalMultiplier((Vector2)transform.up, springForce, horizontalForceMultiplier);
            playerMovement.stunDuration = stunDuration;
            playerMovement.isStunned = true;
            Debug.Log(transform.up);
        }
    }
}
