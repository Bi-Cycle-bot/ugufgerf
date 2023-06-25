// Trent Lucas
// 25 June 2023

// Health pack manager
// heals the player and destroys the health pack
//////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private PlayerMovement player;
    public int healAmount;

    // Called when the health pack has collided with another object
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            // null case checker
            if (player == null) {
                player = collision.gameObject.GetComponent<PlayerMovement>();
            }
            
            player.heal(healAmount);
            Destroy(gameObject);
        }
    }
}
