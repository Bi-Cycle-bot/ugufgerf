// Trent Lucas
// 25 June 2023

// Sets all spawnpoints in the scene
// resets the player to their most recent checkpoint
// spawn and despawn all enemies when player has spawned/respawned
/////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    
    public GameObject player;
    public GameObject enemies;
    private GameObject currentEnemies;
    public Transform[] spawnpoint = new Transform[0];
    [HideInInspector]
    public int checkpoint;

    // Set first cjeckpoint and spawn all enemies
    void Start() {
        checkpoint = 0;
        currentEnemies = Instantiate(enemies, Vector3.zero, Quaternion.identity);
    }

    // Respawn player and despawn and respawn all enemies
    public void respawnAtCheckpoint() {
        player.transform.position = spawnpoint[checkpoint].position;
        Destroy(currentEnemies);
        currentEnemies = Instantiate(enemies, Vector3.zero, Quaternion.identity);
    }
}
