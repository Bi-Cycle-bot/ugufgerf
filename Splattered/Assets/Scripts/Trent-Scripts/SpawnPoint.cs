using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    
    public GameObject player;
    public Transform[] spawnpoint = new Transform[0];
    [HideInInspector]
    public int checkpoint;

    void Start() {
        checkpoint = 0;
    }

    public void respawnAtCheckpoint() {
        player.transform.position = spawnpoint[checkpoint].position;
    }
}
