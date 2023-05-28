using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Collision script for checkpoint triggers
//////////////////////////////////////////////////////////

public class Checkpoint : MonoBehaviour
{
    
    public PlayerMovement player;
    public int checkpointNumber;
    private SpawnPoint checkpointManager;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            checkpointManager = GameObject.Find("Player").GetComponent<SpawnPoint>();
            checkpointManager.checkpoint = checkpointNumber;
            Destroy(gameObject);
        }
    }
}
