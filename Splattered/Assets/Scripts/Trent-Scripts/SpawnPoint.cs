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

    void Start() {
        checkpoint = 0;
        currentEnemies = Instantiate(enemies, Vector3.zero, Quaternion.identity);
    }

    public void respawnAtCheckpoint() {
        player.transform.position = spawnpoint[checkpoint].position;
        Destroy(currentEnemies);
        currentEnemies = Instantiate(enemies, Vector3.zero, Quaternion.identity);
    }
}
