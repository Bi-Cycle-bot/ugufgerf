// Trent Lucas
// 25 June 2023

// Collision script for checkpoint triggers
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public PlayerMovement player;
    public int checkpointNumber;
    private SpawnPoint checkpointManager;

    // Called when the checkpoint has collided with another object
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointManager = GameObject.Find("Player").GetComponent<SpawnPoint>();
            checkpointManager.checkpoint = checkpointNumber;
            Destroy(gameObject);
        }
    }
}
