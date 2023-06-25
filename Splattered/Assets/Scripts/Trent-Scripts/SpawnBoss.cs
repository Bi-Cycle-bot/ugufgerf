// Trent Lucas
// 25 June 2023

// Boss Spawn Manager
//
// Checks if player is in boss spawn zone,
// Spawns all bosses,
// Checks if all bosses have been killed to open exit door,
// Despawns bosses on player death call
//////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBoss : MonoBehaviour
{

    public GameObject player;
    public RabbitBoss boss;
    public Transform spawnPoint;
    public Vector2 minXY;
    public Vector2 maxXY;
    [HideInInspector] public bool spawned;
    [HideInInspector] public RabbitBoss[] bossCopy;
    public GameObject door;
    public int bossCount;
    private bool[] bossCopyBool;
    public GameObject[] spawnpoints;

    // Sets array of boss objects and bool array if each boss has died to the player
    void Start() {
        spawned = false;
        bossCopy = new RabbitBoss[bossCount];
        bossCopyBool = new bool[bossCount];
    }

    // Checks the player is in the boss spawn zone to spawn bosses
    void Update()
    {
        // Check if player position is in the boss spawn zone
        if ((player.transform.position.x > minXY[0] && player.transform.position.x < maxXY[0]) && (player.transform.position.y > minXY[1] && player.transform.position.y < maxXY[1]) && spawned == false) {

            // spawn all bosses
            for (int i = 0; i < bossCopy.Length; i++) {
                bossCopy[i] = Instantiate(boss, spawnpoints[i].transform.position, Quaternion.identity);
            }
            spawned = true;
        }

        // Checks if a boss has died
        for (int i = 0; i < bossCopy.Length; i++) {
            if (bossCopy[i] != null && bossCopy[i].currentHealth <= 0) {
                bossCopyBool[i] = true;
            }
        }

        // Checks if all bosses have died
        if (Array.TrueForAll(bossCopyBool, trueCondition)) {
            door.SetActive(false);
        }
    }

    // Boss death checker helper
    bool trueCondition(bool value) {
        return value == true;
    }

    // Despawn all bosses and reset arrays
    public void DespawnBosses() {
        if (spawned == true) {
            for (int i = 0; i < bossCopy.Length; i++) {
                bossCopy[i].despawn();
            }

            Start();
        }
    }
}
