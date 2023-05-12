using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPad : MonoBehaviour
{
    public float springForce = 30f;
    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerMovement.directionalKnockback(new Vector2(0, 1), springForce);
        }
    }
}
