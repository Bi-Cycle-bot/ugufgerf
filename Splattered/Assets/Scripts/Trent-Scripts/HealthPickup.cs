using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private PlayerMovement player;
    public int healAmount;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            if (player == null) {
                player = collision.gameObject.GetComponent<PlayerMovement>();
            }
            player.heal(healAmount);
            Destroy(gameObject);
        }
    }
}
